using API_FirstJaeger.Infrastructure.Clients;
using Google.Protobuf.WellKnownTypes;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace API_FirstJaeger.Extensions
{
	public static class BuildExtension
	{
		public static void AddTracing(this WebApplicationBuilder builder)
		{
			builder.Services.AddOpenTelemetry().WithTracing(b =>
			{
				b.SetResourceBuilder(
					ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName))
				 .AddAspNetCoreInstrumentation()
				 .AddOtlpExporter(opts => { 
					 opts.Endpoint = new Uri(builder.Configuration["Jaeger:Uri"]!); 
				 });
			});
		}
		public static void AddServices(this WebApplicationBuilder builder)
		{
			builder.Services.AddHttpClient<ISecondJaegerClient, SecondJaegerClient>(nameof(SecondJaegerClient), client =>
			{
				client.BaseAddress = new Uri(builder.Configuration["Clients:API-SecondJaeger:BasePath"]!);
			});
			builder.Services.AddScoped<ISecondJaegerClient, SecondJaegerClient>();
		}
	}
}
