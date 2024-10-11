using API_ErrorHandler.Domain.Interfaces.Services;
using API_ErrorHandler.Service.Services;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using Serilog.Exceptions;
using System.Reflection;


namespace API_ErrorHandler.Extensions
{
	public static class BuildExtension
	{
		public static void AddServices(this WebApplicationBuilder builder)
		{
			builder.Services.AddScoped<ISomeService, SomeService>();
		}

		public static void AddHealthChecks(this WebApplicationBuilder builder)
		{

			builder.Services.AddHealthChecks()
			.AddCheck("self", () => HealthCheckResult.Healthy())
			.AddElasticsearch(builder.Configuration["ElasticConfiguration:Uri"]!, timeout: TimeSpan.FromSeconds(2), name: "elasticsearch", failureStatus: HealthStatus.Unhealthy, tags: new[] { nameof(builder.Environment) }); // Configuração para seu Elasticsearch
		}

		public static void ConfigureLogging(this WebApplicationBuilder builder)
		{
			var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{environment}.json", optional: true)
				.Build();

			Log.Logger = new LoggerConfiguration()
				.Enrich.FromLogContext()
				.Enrich.WithExceptionDetails()
				.WriteTo.Debug()
				.WriteTo.Console()
				.WriteTo.Elasticsearch(new[] { new Uri(configuration["ElasticConfiguration:Uri"]!) }, opts => {
					//Sao 3 parametros nesse configuracao do index:
					// type -> nome do que se refere
					// corpo -> meio do nome
					// namespace -> final
					// Nos exemplos eu to optando por logs- (pois é sobre logs)
					// nome-aplicacao-ambiente- ($"{Assembly.GetExecutingAssembly().GetName().Name!.ToLower().Replace(".", "-")}-{environment!.ToLower()})
					// 2024-10 (DateTime.UtcNow:yyyy-MM)
					opts.DataStream = new DataStreamName("logs", $"{Assembly.GetExecutingAssembly().GetName().Name!.ToLower().Replace(".", "-")}-{environment!.ToLower()}", $"{DateTime.UtcNow:yyyy-MM}");
				})
				.Enrich.WithProperty("Environment", environment)
				.Enrich.WithProperty("HostName", System.Net.Dns.GetHostName())
				.ReadFrom.Configuration(configuration: configuration)
				.Enrich.WithThreadId()
				.CreateLogger();

			builder.Host.UseSerilog();
		}
	}
}
