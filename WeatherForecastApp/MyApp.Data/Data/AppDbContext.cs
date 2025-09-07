using Microsoft.EntityFrameworkCore;
using MyApp.Core.Models; // WeatherLog burada

namespace MyApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<WeatherLog> WeatherLogs { get; set; }
    }
}
