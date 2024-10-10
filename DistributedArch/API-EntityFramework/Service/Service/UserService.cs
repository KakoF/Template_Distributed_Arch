using Domain.Interfaces;
using Domain.Models;
using Domain.Requests;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Service.Service
{
	public class UserService : IUserService
	{
		private readonly AppDataContext _context;
		public UserService(AppDataContext context)
		{
			_context = context;
		}
		public async Task<UserModel> CreateAsync(UserCreateRequest request)
		{
			var user = new UserModel(request.Name);
			await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();
			return user;
		}

		public async Task<bool> DeleteAsync(long id)
		{
			var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
			if (user == null)
				return false;

			_context.Users.Remove(user);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<IEnumerable<UserModel>> GetAsync()
		{
			var query = _context.Users.AsNoTracking();

			var users = await query.ToListAsync();

			return users;
		}

		public async Task<UserModel?> GetAsync(long id)
		{
			var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
			return user;
		}

		public async Task<UserModel?> UpdateAsync(long id, UserCreateRequest request)
		{
			var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
			if (user == null)
				return default;

			user.Update(request.Name);

			_context.Users.Update(user);
			await _context.SaveChangesAsync();
			return user;
		}
	}
}
