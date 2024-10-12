using API_SecondJaeger.Domain.Models;

namespace API_SecondJaeger.Infrastructure.Clients
{
	public interface IAPIEntityFrameworkClient
	{
		Task<UserModel?> GetUser(long id);
	}
	public class APIEntityFrameworkClient : IAPIEntityFrameworkClient
	{
		private readonly HttpClient _client;

		public APIEntityFrameworkClient(IHttpClientFactory httpClientFactory)
		{
			_client = httpClientFactory.CreateClient(nameof(APIEntityFrameworkClient));
		}

		public async Task<UserModel?> GetUser(long id)
		{
			var result = await _client.GetAsync($"User/{id}");
			return await result.Content.ReadFromJsonAsync<UserModel>();
		}
	}
}

