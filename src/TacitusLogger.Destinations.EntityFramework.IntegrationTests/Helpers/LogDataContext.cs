using Microsoft.EntityFrameworkCore; 

namespace TacitusLogger.Destinations.EntityFramework.IntegrationTests
{
    internal class LogModelContext : DbContext
    {
        public LogModelContext(DbContextOptions<LogModelContext> options)
            : base(options)
        {

        }

        public virtual DbSet<LogDbModel> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LogModelConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}