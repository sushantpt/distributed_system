using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// masstransit config
builder.Services.AddMassTransit(busConf =>
{
    busConf.SetKebabCaseEndpointNameFormatter();
    busConf.SetInMemorySagaRepositoryProvider();

    var consumerType = typeof(Program).Assembly;

    busConf.AddConsumers(consumerType);
    busConf.AddSagaStateMachines(consumerType);
    busConf.AddSagas(consumerType);
    busConf.AddActivities(consumerType);

    busConf.UsingRabbitMq((busResCon, busFacConf) =>
    {
        busFacConf.Host(builder.Configuration.GetSection("RabbitMqOptions").GetSection("host").Value!, "/", h =>
        {
            h.Username(builder.Configuration.GetSection("RabbitMqOptions").GetSection("username").Value!);
            h.Password(builder.Configuration.GetSection("RabbitMqOptions").GetSection("password").Value!);
        });

        busFacConf.ConfigureEndpoints(busResCon);
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
