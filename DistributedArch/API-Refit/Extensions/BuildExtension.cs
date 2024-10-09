using API_Refit.Infrastruct.Interfaces;
using API_Refit.Service;
using API_Refit.Service.Interfaces;
using Refit;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace API_Refit.Extensions
{
	public static class BuildExtension
	{
		public static void AddServices(this WebApplicationBuilder builder)
		{
			builder.Services.AddHttpContextAccessor();
			builder.Services.AddTransient<RefitTracingHeaderHandler>();

			builder.Services.AddScoped<IRefitService, RefitService>();

			builder.Services
				.AddRefitClient<IErrorHandlerClient>()
				.ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["Clients:ErrorHandlerUrl"]!))
				.AddHttpMessageHandler<RefitTracingHeaderHandler>();
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
