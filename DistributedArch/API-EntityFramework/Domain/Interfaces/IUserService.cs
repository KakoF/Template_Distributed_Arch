
using Domain.Models;
using Domain.Requests;
using Domain.Requests.QueryParams;

namespace Domain.Interfaces
{
	public interface IUserService
	{
		Task<IEnumerable<UserModel>> GetAsync();
		Task<IEnumerable<UserModel>> PagedAsync(PagedParams parameters);
		Task<UserModel?> GetAsync(long id);
		Task<bool> DeleteAsync(long id);
		Task<UserModel?> UpdateAsync(long id, UserCreateRequest request);
		Task<UserModel> CreateAsync(UserCreateRequest request);
	}
}
