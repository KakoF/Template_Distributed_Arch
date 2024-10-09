namespace API_Refit.Extensions
{
	public class RefitTracingHeaderHandler : DelegatingHandler
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public RefitTracingHeaderHandler(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			// Recuperar o valor do header persistido no HttpContext
			var customHeader = _httpContextAccessor.HttpContext?.Items["TracingId"]?.ToString();

			// Se o valor do header existir, adicioná-lo à requisição
			if (!string.IsNullOrEmpty(customHeader))
			{
				request.Headers.Add("X-Tracing-ID", customHeader);
			}

			return await base.SendAsync(request, cancellationToken);
		}
	}
}