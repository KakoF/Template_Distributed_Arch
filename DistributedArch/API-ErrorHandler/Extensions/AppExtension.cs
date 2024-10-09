using API_ErrorHandler.Middlewares;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Prometheus;
using System.Net;

namespace API_ErrorHandler.Extensions
{
	public static class AppExtension
	{
		public static void AddCustomMiddlewares(this WebApplication app)
		{
			app.UseMiddleware(typeof(RequestTracingMiddleware));
			app.UseMiddleware(typeof(ErrorHandlingMiddleware));
		}
		public static void UsePrometheus(this WebApplication app)
		{
			//Preciso setar essa conf de options, para quando algum serviço externo ficar Unhealthy, não quebrar integração com Prometheus
			app.UseHealthChecksPrometheusExporter("/metrics", options => options.ResultStatusCodes[HealthStatus.Unhealthy] = (int)HttpStatusCode.OK);
			app.UseHttpMetrics();
		}
	}
}