using API_FirstJaeger.Domain.Models;

namespace API_FirstJaeger.Infrastructure.Clients
{
	public interface ISecondJaegerClient
	{
		Task<UserModel?> GetUser(long id);
	}
	public class SecondJaegerClient: ISecondJaegerClient
	{
		private readonly HttpClient _client;

		public SecondJaegerClient(IHttpClientFactory httpClientFactory)
		{
			_client = httpClientFactory.CreateClient(nameof(SecondJaegerClient));
		}

		public async Task<UserModel?> GetUser(long id)
		{
			var result = await _client.GetAsync($"User/{id}");
			return await result.Content.ReadFromJsonAsync<UserModel>();
		}
	}
}
