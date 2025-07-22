using StackExchange.Redis;

var redis = ConnectionMultiplexer.Connect("localhost:6379,password=Admin@#,ConnectTimeout=10000");
var sub = redis.GetSubscriber();

sub.Subscribe("canal-mensagens", (channel, message) =>
{
	Console.WriteLine($"Recebido: {message}");
});

Console.WriteLine("Assinando canal... Pressione qualquer tecla para sair.");
Console.ReadKey();