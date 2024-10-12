using API_SecondJaeger.Infrastructure.Clients;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace API_SecondJaeger.Extensions
{
	public static class BuildExtension
	{

		public static void AddTracing(this WebApplicationBuilder builder)
		{
			builder.Services.AddOpenTelemetry().WithTracing(b => {
				b.SetResourceBuilder(
					ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName))
				 .AddAspNetCoreInstrumentation()
				 .AddOtlpExporter(opts => { opts.Endpoint = new Uri(builder.Configuration["Jaeger:Uri"]!); });
			});
		}

		public static void AddServices(this WebApplicationBuilder builder)
		{
			builder.Services.AddHttpClient<IAPIEntityFrameworkClient, APIEntityFrameworkClient>(nameof(APIEntityFrameworkClient), client =>
			{
				client.BaseAddress = new Uri(builder.Configuration["Clients:APIEntityFramework:BasePath"]!);
			});
			builder.Services.AddScoped<IAPIEntityFrameworkClient, APIEntityFrameworkClient>();
		}
	}
}
