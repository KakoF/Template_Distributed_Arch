using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using System;

namespace WebApi.Middleware
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;

		public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (DomainException ex)
			{
				_logger.LogWarning(ex, ex.Message);
				
				var exception = (DomainException)ex;
				context.Response.StatusCode = exception.StatusCode;
				await context.Response.WriteAsJsonAsync(new { message = ex.Message });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				context.Response.StatusCode = StatusCodes.Status500InternalServerError;
				await context.Response.WriteAsJsonAsync(new { message = "Internal Server Error" });
			}
		}
	}

}
