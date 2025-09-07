using MyApp.Core.Models;

namespace MyApp.Infrastructure.Services
{
    public interface IWeatherService
    {
        Task<WeatherDto?> GetWeatherAsync(string city = "Istanbul");
    }
}
