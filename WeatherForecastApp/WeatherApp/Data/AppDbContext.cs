using Microsoft.EntityFrameworkCore;
using MyApp.Core.Models;

namespace WeatherApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<WeatherLog> WeatherLogs { get; set; }
    }
}
