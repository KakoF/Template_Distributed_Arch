using API_Refit.Middlewares;

namespace API_Refit.Extensions
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