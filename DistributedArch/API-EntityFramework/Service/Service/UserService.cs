using Domain.Interfaces;
using Domain.Models;
using Domain.Requests;
using Domain.Requests.QueryParams;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Service.Service
{
	public class UserService : IUserService
	{
		private readonly IDbContextFactory<AppDataContext> _contextFactory;
		public UserService(IDbContextFactory<AppDataContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public async Task<UserModel> CreateAsync(UserCreateRequest request)
		{
			var user = new UserModel(request.Name);
			using (var context = _contextFactory.CreateDbContext())
			{
				await context.Users.AddAsync(user);
				await context.SaveChangesAsync();
			}
			return user;
		}


		public async Task<bool> DeleteAsync(long id)
		{
			using (var context = _contextFactory.CreateDbContext())
			{
				var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
				if (user == null)
					return false;

				context.Users.Remove(user);
				await context.SaveChangesAsync();
				return true;
			}
		}

		public async Task<IEnumerable<UserModel>> GetAsync()
		{
			using (var context = _contextFactory.CreateDbContext())
			{
				var query = context.Users.AsNoTracking();

				var users = await query.ToListAsync();

				return users;
			}
		}


		public async Task<IEnumerable<UserModel>> PagedAsync(PagedParams parameters)
		{
			Console.WriteLine(parameters.Page);
			using (var context = _contextFactory.CreateDbContext())
			{
				var query = context.Users.AsNoTracking();

				var users = await query.Skip((parameters.Page - 1) * parameters.PageSize).Take(parameters.PageSize).ToListAsync();

				return users;
			}
		}



		public async Task<UserModel?> GetAsync(long id)
		{
			using (var context = _contextFactory.CreateDbContext())
			{
				var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
				return user;
			}
			
		}

		public async Task<UserModel?> UpdateAsync(long id, UserCreateRequest request)
		{
			using (var context = _contextFactory.CreateDbContext())
			{
				var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
				if (user == null)
					return default;

				user.Update(request.Name);

				context.Users.Update(user);
				await context.SaveChangesAsync();
				return user;
			}
		}
	}
}
