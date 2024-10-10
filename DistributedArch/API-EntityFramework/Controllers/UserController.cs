using Domain.Interfaces;
using Domain.Models;
using Domain.Requests;
using Microsoft.AspNetCore.Mvc;

namespace API_EntityFramework.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IUserService _service;
		public UserController(IUserService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IEnumerable<UserModel>> GetAsync()
		{
			return await _service.GetAsync();
		}
		
		[HttpGet]
		[Route("{id}")]
		public async Task<UserModel?> GetAsync(long id)
		{
			return await _service.GetAsync(id);
		}

		[HttpPost]
		public async Task<UserModel> CreateAsync([FromBody] UserCreateRequest request)
		{
			return await _service.CreateAsync(request);
		}


		[HttpPut]
		[Route("{id}")]
		public async Task<UserModel?> UpdateAsync(long id, [FromBody] UserCreateRequest request)
		{
			return await _service.UpdateAsync(id, request);
		}

		[HttpDelete]
		[Route("{id}")]
		public async Task<bool> DeleteAsync(long id)
		{
			return await _service.DeleteAsync(id);
		}
	}
}
