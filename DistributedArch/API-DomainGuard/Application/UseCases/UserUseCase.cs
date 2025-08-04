using Domain.Exceptions;
using Domain.Interfaces.Application.UseCases;
using Domain.Models;
using Domain.Records.Requests.User;
using Domain.Records.Responses.User;

namespace Application.UseCases
{
	public class UserUseCase : IUserUseCase
	{
		public UserResponse Create(CreateUserRequest request)
		{
			var user = User.Create(request.Name);

			if (user.Name == "name")
				throw new DomainException($"User cannot be name");

			return new UserResponse(user.Id, user.Name);
		}

		public UserResponse Get(Guid id)
		{
			var user = User.Clone(id, "Mock User");

			if (user == null)
				throw new DomainException($"User not found", 404);

			return new UserResponse(user.Id, user.Name);
		}
	}
}
