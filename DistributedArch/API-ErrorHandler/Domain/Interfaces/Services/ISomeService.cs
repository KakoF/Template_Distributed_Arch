namespace API_ErrorHandler.Domain.Interfaces.Services
{
	public interface ISomeService
	{
		string Get();
		string GetMockErro(int statusCode, string erroMessage);
		string GetMockErro(int statusCode, string erroMessage, List<string> erros);
		double DoDivision(string number);
	}
}
