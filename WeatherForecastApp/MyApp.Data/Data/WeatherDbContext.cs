using Microsoft.EntityFrameworkCore;
using MyApp.Core.Models;

namespace MyApp.Data
{
    public class WeatherDbContext : DbContext
    {
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options)
            : base(options) { }

        public DbSet<WeatherLog> WeatherLogs { get; set; }
    }
}
