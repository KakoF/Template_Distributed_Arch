using API_FirstJaeger.Domain.Models;
using API_FirstJaeger.Infrastructure.Clients;
using Microsoft.AspNetCore.Mvc;

namespace API_FirstJaeger.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		private readonly ISecondJaegerClient _client;

		public UserController(ISecondJaegerClient client)
		{
			_client = client;
		}

		[HttpGet]
		[Route("{id}")]
		public async Task<UserModel?> GetAsync(long id)
		{
			return await _client.GetUser(id);
		}
	}
}
