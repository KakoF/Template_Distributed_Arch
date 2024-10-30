using MassTransit;
using RabbitMasstransitConsumer.Workers;

namespace RabbitMasstransitConsumer.Extensions
{
	public static class BuilderExtensions
	{
		public static void AddMassTransitRabbitMq(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddMassTransit(busConfigurator =>
			{
				busConfigurator.SetSnakeCaseEndpointNameFormatter();
				busConfigurator.AddConsumer<UserDirectEventConsumer>();
				busConfigurator.UsingRabbitMq((ctx, cfg) =>
				{
					cfg.Host(configuration.GetConnectionString("RabbitMq"));
					cfg.ConfigureEndpoints(ctx, new SnakeCaseEndpointNameFormatter(false));
					cfg.UseMessageRetry(retry => { retry.Interval(3, TimeSpan.FromSeconds(5)); });


					cfg.ReceiveEndpoint("user_direct_queue", e =>
					{
						e.Bind("amq.fanout", s =>
						{
							s.ExchangeType = "fanout";
						});

						e.ConfigureConsumer<UserDirectEventConsumer>(ctx);
					});
				});

			});
		}
	}
}
