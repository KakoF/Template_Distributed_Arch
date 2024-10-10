using API_EntityFramework.Infrastructure;
using API_EntityFramework.Services.Interfaces;
using API_EntityFramework.Services.Service;
using Microsoft.EntityFrameworkCore;

namespace API_EntityFramework.Extensions
{
	public static class BuildExtension
	{
		public static void AddServices(this WebApplicationBuilder builder)
		{
			builder.Services.AddScoped<IUserService, UserService>();
		}
		public static void AddConfiguration(this WebApplicationBuilder builder)
		{
			ApiConfiguration.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
		}

		public static void AddDataContexts(this WebApplicationBuilder builder)
		{
			builder
				.Services
				.AddDbContext<AppDataContext>(
					x =>
					{
						x.UseSqlServer(ApiConfiguration.ConnectionString);
					});

		}
	}
}