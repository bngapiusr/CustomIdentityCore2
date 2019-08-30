using CustomIdentityCore2.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomIdentityCore2.Data
{
    public class CustomIdentityCoreDbContext : DbContext
    {
        public CustomIdentityCoreDbContext(DbContextOptions<CustomIdentityCoreDbContext> options): base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
    }
}