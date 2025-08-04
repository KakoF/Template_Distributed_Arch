using Domain.Records.Requests.User;
using Domain.Records.Responses.User;

namespace Domain.Interfaces.Application.UseCases
{
	public interface IUserUseCase
	{
		public UserResponse Create(CreateUserRequest request);
		public UserResponse Get(Guid id);
	}
}
