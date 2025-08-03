using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Sinks.Grafana.Loki;
using System.Diagnostics;
var builder = WebApplication.CreateBuilder(args);


var activitySource = new ActivitySource("API-OpenTelemetry");
builder.Services.AddOpenTelemetry()
	.ConfigureResource(resource => resource.AddService("API-OpenTelemetry"))
	.WithTracing(tracing =>
	{
		tracing
			.AddSource("API-OpenTelemetry") // Registra sua fonte
			.AddAspNetCoreInstrumentation()
			.AddHttpClientInstrumentation()
			.AddOtlpExporter(options =>
			{
				options.Endpoint = new Uri("http://localhost:4317");
			});
	});


Log.Logger = new LoggerConfiguration()
	.MinimumLevel.Information()
	.Enrich.FromLogContext()
	.Enrich.WithSpan()
	//.WriteTo.Console() // Opcional: manter logs no console
	.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] TraceId: {TraceId} | {Message:lj}{NewLine}{Exception}")
	.WriteTo.GrafanaLoki("http://localhost:3100", labels: new[] {
		new LokiLabel { Key = "service_name", Value = "API-OpenTelemetry" },
		new LokiLabel { Key = "Environment", Value = builder.Environment.EnvironmentName },
		new LokiLabel { Key = "trace_id", Value = "{TraceId}" }
	})
	.CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();
