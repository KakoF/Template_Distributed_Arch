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
		private readonly ILogger<UserController> _logger;
		public UserController(IUserService service, ILogger<UserController> logger)
		{
			_service = service;
			_logger = logger;
		}

		[HttpGet]
		public async Task<IEnumerable<UserModel>> GetAsync()
		{
			_logger.LogInformation(nameof(GetAsync));
			return await _service.GetAsync();
		}

		[HttpGet]
		[Route("{id}")]
		public async Task<UserModel?> GetAsync(long id)
		{
			_logger.LogInformation(nameof(GetAsync));
			return await _service.GetAsync(id);
		}

		[HttpPost]
		public async Task<UserModel> CreateAsync([FromBody] UserCreateRequest request)
		{
			_logger.LogInformation(nameof(CreateAsync));
			return await _service.CreateAsync(request);
		}


		[HttpPut]
		[Route("{id}")]
		public async Task<UserModel?> UpdateAsync(long id, [FromBody] UserCreateRequest request)
		{
			_logger.LogInformation(nameof(UpdateAsync));
			return await _service.UpdateAsync(id, request);
		}

		[HttpDelete]
		[Route("{id}")]
		public async Task<bool> DeleteAsync(long id)
		{
			_logger.LogInformation(nameof(DeleteAsync));
			return await _service.DeleteAsync(id);
		}
	}
}

