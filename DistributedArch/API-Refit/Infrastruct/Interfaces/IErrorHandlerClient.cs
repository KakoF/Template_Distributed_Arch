using Refit;

namespace API_Refit.Infrastruct.Interfaces
{
	public interface IErrorHandlerClient
	{
		[Get("/ErrorHandler")]
		Task<string> GetAsync();

		[Get("/ErrorHandler/DoDivision/{number}")]
		Task<double> DoDivisionAsync(string number);
	}
}
