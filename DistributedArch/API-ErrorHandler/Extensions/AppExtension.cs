using API_ErrorHandler.Middlewares;
using Prometheus;

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
			app.UseHealthChecksPrometheusExporter("/metrics");
			app.UseHttpMetrics();
		}
	}
}