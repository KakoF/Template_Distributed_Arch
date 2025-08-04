using Domain.Interfaces.Application.UseCases;
using Domain.Records.Requests.User;
using Domain.Records.Responses.User;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{

		private readonly IUserUseCase _useCase;

		public UserController(IUserUseCase useCase)
		{
			_useCase = useCase;
		}

		[HttpGet("{id}")]
		public UserResponse Get(Guid id)
		{
			return _useCase.Get(id);
		}

		[HttpPost]
		public UserResponse Post([FromBody] CreateUserRequest request)
		{
			return _useCase.Create(request);
		}
	}
}
