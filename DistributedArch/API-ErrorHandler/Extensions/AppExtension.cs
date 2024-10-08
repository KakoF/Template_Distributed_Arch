using API_ErrorHandler.Middlewares;

namespace API_ErrorHandler.Extensions
{
	public static class AppExtension
	{
		public static void AddCustomMiddlewares(this WebApplication app)
		{
			app.UseMiddleware(typeof(RequestTracingMiddleware));
			app.UseMiddleware(typeof(ErrorHandlingMiddleware));
		}
	}
}