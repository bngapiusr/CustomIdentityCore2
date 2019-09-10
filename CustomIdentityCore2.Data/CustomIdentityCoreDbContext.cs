using CustomIdentityCore2.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

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

        public static readonly LoggerFactory IdentityLoggerFactory
            = new LoggerFactory(new[] {
                new ConsoleLoggerProvider( (category, level) 
                     => category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information,true) });

    }
}