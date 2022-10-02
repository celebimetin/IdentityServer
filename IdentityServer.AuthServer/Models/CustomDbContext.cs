using Microsoft.EntityFrameworkCore;

namespace IdentityServer.AuthServer.Models
{
    public class CustomDbContext : DbContext
    {
        public CustomDbContext(DbContextOptions<CustomDbContext> options) : base (options) { }

        public DbSet<CustomUser> CustomUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomUser>().HasData(new CustomUser() { Id = 1, UserName = "mcelebi", Email = "mcelebi@gmail.com", Password = "password", City = "istanbul" });
            base.OnModelCreating(modelBuilder);
        }
    }
}