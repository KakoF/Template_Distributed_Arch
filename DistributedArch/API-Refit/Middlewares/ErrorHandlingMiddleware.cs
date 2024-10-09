using System.Net;

namespace API_Refit.Middlewares
{
	public class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate next;
		private readonly ILogger<ErrorHandlingMiddleware> _logger;

		public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
		{
			this.next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		private Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			_logger.LogError(exception, exception.Message);
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			return context.Response.WriteAsync(exception.Message);
		}
	}
}

