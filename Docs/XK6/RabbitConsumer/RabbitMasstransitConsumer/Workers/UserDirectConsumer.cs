using MassTransit;
using RabbitMasstransitConsumer.Events;

namespace RabbitMasstransitConsumer.Workers
{
	public class UserDirectEventConsumer : IConsumer<UserDirectEvent>
	{
		private readonly ILogger<UserDirectEventConsumer> _logger;

		public UserDirectEventConsumer(ILogger<UserDirectEventConsumer> logger)
		{
			_logger = logger;
		}
		public Task Consume(ConsumeContext<UserDirectEvent> context)
		{
			_logger.LogInformation($"User message receive: {context.Message.Name}");
			return Task.CompletedTask;
		}
	}
}