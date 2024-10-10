using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
	public class AppDataContext : DbContext
	{

		public DbSet<UserModel> Users { get; set; } = null!;

		public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserModel>().ToTable("User");
		}
	}
}
