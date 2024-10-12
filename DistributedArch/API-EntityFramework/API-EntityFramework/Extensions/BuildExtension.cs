using Domain.Interfaces;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using Serilog.Exceptions;
using Service.Service;
using System.Reflection;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;

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
			.AddUrlGroup(new Uri(builder.Configuration["Jaeger:HealthCheck"]!), timeout: TimeSpan.FromSeconds(2), name: "jaeger", failureStatus: HealthStatus.Unhealthy)
			.AddDbContextCheck<AppDataContext>();
		}
		public static void AddTracing(this WebApplicationBuilder builder)
		{
			builder.Services.AddOpenTelemetry().WithTracing(b => {
				b.SetResourceBuilder(
					ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName))
				 .AddAspNetCoreInstrumentation()
				 .AddOtlpExporter(opts => { opts.Endpoint = new Uri(builder.Configuration["Jaeger:Uri"]!); });
			});
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
				//.WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment!))
				.Enrich.WithProperty("Environment", environment)
				.Enrich.WithProperty("HostName", System.Net.Dns.GetHostName())
				.ReadFrom.Configuration(configuration: configuration)
				.Enrich.WithThreadId()
				.CreateLogger();

			builder.Host.UseSerilog();
		}
	}
}