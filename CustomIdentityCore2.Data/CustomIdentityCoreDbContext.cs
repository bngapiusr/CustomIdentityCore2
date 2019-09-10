using CustomIdentityCore2.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace CustomIdentityCore2.Data
{
    public class CustomIdentityCoreDbContext : DbContext
    {
        private ILoggerFactory _loggerFactory;
        public CustomIdentityCoreDbContext(DbContextOptions<CustomIdentityCoreDbContext> options): base(options)
        {
        }

        public CustomIdentityCoreDbContext()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder => builder
                .AddConsole()
                .AddFilter(DbLoggerCategory.Database.Command.Name,
                    LogLevel.Information));
            _loggerFactory = serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();
        }
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(_loggerFactory)
                .EnableSensitiveDataLogging(true);
        }

        //old way of logging in EFCore 2.0 ...
        //public static readonly LoggerFactory IdentityLoggerFactory
        //    = new LoggerFactory(new[] {
        //        new ConsoleLoggerProvider( (category, level) 
        //             => category == DbLoggerCategory.Database.Command.Name
        //            && level == LogLevel.Information,true) });

        //Resource Link... https://msdn.microsoft.com/magazine/mt830355

        //private ILoggerFactory getLoggerFactory()
        //{
        //    IServiceCollection serviceCollection = new ServiceCollection();
        //    serviceCollection.AddLogging(builder => builder
        //        .AddConsole()
        //        .AddFilter(DbLoggerCategory.Database.Command.Name,
        //            LogLevel.Information));
        //    return serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();
        //}

    }
}