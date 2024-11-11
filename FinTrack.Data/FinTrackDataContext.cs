using FinTrack.Data.Contracts;
using FinTrack.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace FinTrack.Data
{
    public class FinTrackDataContext : DbContext, IDataContext
    {
        protected readonly IConfiguration Configuration;
        private DbConnectionSettings _settings;
        public DbSet<User> Users { get; set; }
        public DbSet<Currency> Currencies { get; set; }

        public FinTrackDataContext(DbConnectionSettings settings)
        {
            _settings = settings;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_settings.MSSQLDatabase);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
