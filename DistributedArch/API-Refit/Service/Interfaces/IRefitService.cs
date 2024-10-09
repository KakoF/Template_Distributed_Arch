namespace API_Refit.Service.Interfaces
{
	public interface IRefitService
	{
		Task<string> GetAsync();
		Task<double> DoDivisionAsync(string number);
	}
}
