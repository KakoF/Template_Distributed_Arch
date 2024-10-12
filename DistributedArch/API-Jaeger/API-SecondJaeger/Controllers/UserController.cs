using API_SecondJaeger.Domain.Models;
using API_SecondJaeger.Infrastructure.Clients;
using Microsoft.AspNetCore.Mvc;

namespace API_SecondJaeger.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IAPIEntityFrameworkClient _client;

		public UserController(IAPIEntityFrameworkClient client)
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
