using Domain.Interfaces;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using Service.Service;
using System.Reflection;

namespace API_EntityFramework.Extensions
{
	public static class BuildExtension
	{
		public static void AddServices(this WebApplicationBuilder builder)
		{
			builder.Services.AddScoped<IUserService, UserService>();
		}
		public static void AddConfiguration(this WebApplicationBuilder builder)
		{
			ApiConfiguration.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
		}

		public static void AddDataContexts(this WebApplicationBuilder builder)
		{
			builder
				.Services
				.AddDbContext<AppDataContext>(
					x =>
					{
						x.UseSqlServer(ApiConfiguration.ConnectionString);
					});

		}

		public static void AddHealthChecks(this WebApplicationBuilder builder)
		{

			builder.Services.AddHealthChecks()
			.AddCheck("self", () => HealthCheckResult.Healthy())
			.AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!, timeout: TimeSpan.FromSeconds(2), name: "sqlServer", failureStatus: HealthStatus.Unhealthy, tags: new[] { nameof(builder.Environment) }); // Configuração para seu Elasticsearch
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