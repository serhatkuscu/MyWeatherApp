using MyApp.Core.Models;

namespace MyApp.Core.Services
{
    public interface IWeatherService
    {
        Task<WeatherDto?> GetWeatherAsync(string city = "Istanbul");
    }
}
