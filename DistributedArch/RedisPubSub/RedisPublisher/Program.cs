using StackExchange.Redis;

var redis = await ConnectionMultiplexer.ConnectAsync("localhost:6379,password=Admin@#,ConnectTimeout=10000");
var pub = redis.GetSubscriber();

for (int i = 1; i <= 5; i++)
{
	await pub.PublishAsync("canal-mensagens", $"Mensagem {i}");
	Console.WriteLine($"Mensagem {i} publicada.");
	await Task.Delay(1000);
}
