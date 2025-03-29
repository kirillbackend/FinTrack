using FinTrack.Data.Contracts;
using FinTrack.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace FinTrack.Data
{
    public class FinTrackDataContext : DbContext, IDataContext
    {
        private DbConnectionSettings _settings;
        protected readonly IConfiguration Configuration;

        public DbSet<User> Users { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Finance> Finances { get; set; }
        public DbSet<AuthToken> AuthTokens { get; set; }

        public FinTrackDataContext(DbConnectionSettings settings)
        {
            _settings = settings;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                _settings.MSSQLDatabase,
                options => options.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
