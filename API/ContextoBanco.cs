using Microsoft.EntityFrameworkCore;

namespace API
{
    public class ContextoBanco(IConfiguration configuration) : DbContext
    {
        public virtual DbSet<WeatherForecast> WeatherForecasts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherForecast>().HasKey(p => p.Id);
        }
    }
}
