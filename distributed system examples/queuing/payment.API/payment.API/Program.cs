using MassTransit;
using Microsoft.Extensions.Caching.Memory;
using payment.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.AddMemoryCache(opt => new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1)));

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// add masstransit
builder.Services.AddMassTransit(busConf =>
{
    busConf.SetKebabCaseEndpointNameFormatter();
    busConf.AddConsumers(typeof(Program).Assembly);
    busConf.UsingRabbitMq((busResCon, busFacConf) =>
    {
        busFacConf.Host(builder.Configuration.GetSection("RabbitMqOptions").GetSection("host").Value!, "/", h =>
        {
            h.Username(builder.Configuration.GetSection("RabbitMqOptions").GetSection("username").Value!);
            h.Password(builder.Configuration.GetSection("RabbitMqOptions").GetSection("password").Value!);
        });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
