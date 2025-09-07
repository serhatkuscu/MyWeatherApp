using Microsoft.AspNetCore.Mvc;
using MyApp.Infrastructure.Services;

namespace MyApp.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWeatherService _weatherService;

        public HomeController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public async Task<IActionResult> Index(string? city)
        {
            if (string.IsNullOrWhiteSpace(city))
                city = "Istanbul";

            var weatherData = await _weatherService.GetWeatherAsync(city);

            // 🔹 Konsola ikon kodunu yaz
            Console.WriteLine($"[DEBUG] City: {weatherData?.City}, Icon: {weatherData?.WeatherIcon}, Desc: {weatherData?.Description}");

            return View(weatherData);
        }

    }
}
