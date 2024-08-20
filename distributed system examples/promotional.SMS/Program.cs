using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// configure mass transit
builder.Services.AddMassTransit(busConf =>
{
    busConf.SetKebabCaseEndpointNameFormatter();
    busConf.SetInMemorySagaRepositoryProvider();

    var consumerType = typeof(Program).Assembly;

    busConf.AddConsumers(consumerType);
    busConf.AddSagas(consumerType);
    busConf.AddActivities(consumerType);

    busConf.UsingRabbitMq((busResCon, busFacConf) =>
    {
        busFacConf.Host(builder.Configuration.GetSection("RabbitMqOptions").GetSection("host").Value!, "/", hConf =>
        {
            hConf.Username(builder.Configuration.GetSection("RabbitMqOptions").GetSection("username").Value!);
            hConf.Password(builder.Configuration.GetSection("RabbitMqOptions").GetSection("password").Value!);
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

await app.RunAsync();
