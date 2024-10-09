using API_ErrorHandler.Domain.Exceptions;
using API_ErrorHandler.Domain.Interfaces.Services;

namespace API_ErrorHandler.Service.Services
{
	public class SomeService : ISomeService
	{
		private readonly ILogger<SomeService> _logger;
		public SomeService(ILogger<SomeService> logger)
		{
			_logger = logger;
		}

		public double DoDivision(string number)
		{
			_logger.LogInformation("Execute DoDivision Method");
			return Convert.ToDouble(number) / 2;
		}

		public string Get()
		{
			_logger.LogInformation("Execute Get Method");
			throw new DomainException("Generic Error");
			return "string";
		}

		public string GetMockErro(int statusCode, string erroMessage)
		{
			_logger.LogInformation("Execute GetMockErro Method");
			throw new DomainException(erroMessage, statusCode);
			return "string";
		}

		public string GetMockErro(int statusCode, string erroMessage, List<string> erros)
		{
			_logger.LogInformation("Execute GetMockErro Method");
			throw new DomainException(erroMessage, erros, statusCode);
			return "string";
		}
	}
}
