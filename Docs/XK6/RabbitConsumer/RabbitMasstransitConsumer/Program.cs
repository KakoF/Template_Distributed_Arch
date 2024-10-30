using RabbitMasstransitConsumer;
using RabbitMasstransitConsumer.Extensions;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddMassTransitRabbitMq(builder.Configuration);

var host = builder.Build();
host.Run();
