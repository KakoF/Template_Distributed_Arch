using Serilog.Context;

namespace API_EntityFramework.Middlewares
{
	public class RequestTracingMiddleware
	{
		private readonly RequestDelegate _next;

		public RequestTracingMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			// Tenta pegar o correlation ID do header ou gera um novo
			var tracingId = context.Request.Headers["X-Tracing-ID"].FirstOrDefault() ?? Guid.NewGuid().ToString();
			// Adiciona o correlation ID ao contexto de logging
			using (LogContext.PushProperty("TracingId", tracingId))
			{
				// Adiciona o correlation ID aos headers HTTP para serviços downstream
				context.Response.Headers.Append("X-Tracing-ID", tracingId);

				// Chama o próximo middleware na pipeline
				await _next(context);
			}

		}
	}
}

