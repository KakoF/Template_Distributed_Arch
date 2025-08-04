using Application.UseCases;
using Domain.Interfaces.Application.UseCases;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Sinks.Grafana.Loki;
using System.Diagnostics;
using WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserUseCase, UserUseCase>();
builder.Services.AddScoped<IOrderUseCase, OderUseCase>();


var activitySource = new ActivitySource("API-DomainGuard");
builder.Services.AddOpenTelemetry()
	.ConfigureResource(resource => resource.AddService("API-DomainGuard"))
	.WithTracing(tracing =>
	{
		tracing
			.AddSource("API-DomainGuard") // Registra sua fonte
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
		new LokiLabel { Key = "service_name", Value = "API-DomainGuard" },
		new LokiLabel { Key = "Environment", Value = builder.Environment.EnvironmentName },
		new LokiLabel { Key = "trace_id", Value = "{TraceId}" }
	})
	.CreateLogger();

builder.Host.UseSerilog();

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

app.UseMiddleware<ExceptionHandlingMiddleware>();


app.Run();
