using API_ErrorHandler.Domain.Interfaces.Services;
using API_ErrorHandler.Service.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
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
			.AddElasticsearch(builder.Configuration["ElasticConfiguration:Uri"]!, name: "elasticsearch", failureStatus: HealthStatus.Unhealthy, tags: new[] { nameof(builder.Environment) }); // Configuração para seu Elasticsearch
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
				.WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment!))
				.Enrich.WithProperty("Environment", environment)
				.Enrich.WithProperty("HostName", System.Net.Dns.GetHostName())
				.ReadFrom.Configuration(configuration: configuration)
				.Enrich.WithThreadId()
				.CreateLogger();

			builder.Host.UseSerilog();
		}

		public static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
		{
			return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]!))
			{
				AutoRegisterTemplate = true,
				IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name!.ToLower().Replace(".", "-")}-{environment.ToLower()}-{DateTime.UtcNow:yyyy-MM}",
				NumberOfReplicas = 1,
				NumberOfShards = 2,
			};
		}
	}
}
