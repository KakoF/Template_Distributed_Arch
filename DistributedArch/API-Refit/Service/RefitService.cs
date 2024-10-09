using API_Refit.Infrastruct.Interfaces;
using API_Refit.Service.Interfaces;
using Refit;

namespace API_Refit.Service
{
	public class RefitService : IRefitService
	{
		private readonly IErrorHandlerClient _client;
		private readonly ILogger<RefitService> _logger;
		public RefitService(IErrorHandlerClient client, ILogger<RefitService> logger)
		{
			_client = client;
			_logger = logger;
		}

		public async Task<double> DoDivisionAsync(string number)
		{
			try
			{
				_logger.LogInformation("Called DoDivisionAsync Method");
				return await _client.DoDivisionAsync(number);
			}
			catch (ApiException ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<string> GetAsync()
		{
			try
			{
				_logger.LogInformation("Called GetAsync Method");
				return await _client.GetAsync();
			}
			catch (ApiException ex)
			{
				throw new Exception(ex.Message);
			}
			
		}
	}
}
