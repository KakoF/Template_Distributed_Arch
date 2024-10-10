
using Domain.Models;
using Domain.Requests;

namespace Domain.Interfaces
{
	public interface IUserService
	{
		Task<IEnumerable<UserModel>> GetAsync();
		Task<UserModel?> GetAsync(long id);
		Task<bool> DeleteAsync(long id);
		Task<UserModel?> UpdateAsync(long id, UserCreateRequest request);
		Task<UserModel> CreateAsync(UserCreateRequest request);
	}
}
