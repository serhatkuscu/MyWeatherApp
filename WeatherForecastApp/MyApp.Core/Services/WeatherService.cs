using System.Net.Http;
using System.Text.Json;
using MyApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using WeatherApp.Data; // DbContext burada

namespace MyApp.Core.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly string _apiKey = "151496c6080389f2d40734a6f41c77a7";
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _db; // EF Core DbContext

        // DI ile AppDbContext gelecek
        public WeatherService(AppDbContext db)
        {
            _httpClient = new HttpClient();
            _db = db;
        }

        public async Task<WeatherDto?> GetWeatherAsync(string city = "Istanbul")
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(city)}&appid={_apiKey}&units=metric&lang=tr";
            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            // Önce "cod" alanına bak
            var cod = root.TryGetProperty("cod", out var codProp) ? codProp.ToString() : "";
            if (cod != "200")
            {
                var message = root.TryGetProperty("message", out var msgProp) ? msgProp.GetString() : "Bilinmeyen hata";
                if (cod == "404")
                    return new WeatherDto { ErrorMessage = "❌ Şehir bulunamadı, lütfen tekrar deneyin." };
                else if (cod == "401")
                    return new WeatherDto { ErrorMessage = "❌ API anahtarı geçersiz. Lütfen kontrol edin." };
                else
                    return new WeatherDto { ErrorMessage = $"❌ Hata: {message}" };
            }

            // Yardımcı metodlar
            string? GetWindDirection(int? deg)
            {
                if (!deg.HasValue) return null;
                string[] directions = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE",
                                        "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW" };
                int index = (int)Math.Round(((double)deg / 22.5) % 16);
                return directions[index];
            }

            DateTime? ConvertUnixToLocal(long? unix, int timezoneOffsetSeconds)
            {
                if (!unix.HasValue) return null;
                var utc = DateTimeOffset.FromUnixTimeSeconds(unix.Value);
                return utc.ToOffset(TimeSpan.FromSeconds(timezoneOffsetSeconds)).DateTime;
            }

            // Main verileri
            var main = root.GetProperty("main");
            var weatherArray = root.GetProperty("weather")[0];

            // Rüzgar verileri
            int? windDeg = null;
            decimal windSpeed = 0;
            if (root.TryGetProperty("wind", out var wind))
            {
                if (wind.TryGetProperty("deg", out var degProp))
                    windDeg = degProp.GetInt32();
                if (wind.TryGetProperty("speed", out var speedProp))
                    windSpeed = speedProp.GetDecimal();
            }

            // Sys verileri
            string country = "";
            long? sunriseUnix = null;
            long? sunsetUnix = null;
            if (root.TryGetProperty("sys", out var sys))
            {
                country = sys.TryGetProperty("country", out var countryProp) ? countryProp.GetString() ?? "" : "";
                sunriseUnix = sys.TryGetProperty("sunrise", out var sunriseProp) ? sunriseProp.GetInt64() : null;
                sunsetUnix = sys.TryGetProperty("sunset", out var sunsetProp) ? sunsetProp.GetInt64() : null;
            }

            // Timezone
            int timezoneOffset = root.TryGetProperty("timezone", out var tzProp) ? tzProp.GetInt32() : 0;

            var dto = new WeatherDto
            {
                City = city,
                Country = country,
                Temperature = main.GetProperty("temp").GetDecimal(),
                FeelsLike = main.GetProperty("feels_like").GetDecimal(),
                Humidity = main.GetProperty("humidity").GetInt32(),
                Pressure = main.GetProperty("pressure").GetInt32(),
                Description = weatherArray.GetProperty("description").GetString() ?? "",
                WeatherIcon = weatherArray.GetProperty("icon").GetString() ?? "",
                WindSpeed = windSpeed,
                WindDeg = windDeg,
                WindDirection = GetWindDirection(windDeg),
                SunriseLocal = ConvertUnixToLocal(sunriseUnix, timezoneOffset),
                SunsetLocal = ConvertUnixToLocal(sunsetUnix, timezoneOffset)
            };

            // ✅ LOG tabloya kaydet
            var log = new WeatherLog
            {
                City = dto.City,
                Temperature = dto.Temperature,
                RequestedAt = DateTime.Now
            };

            _db.WeatherLogs.Add(log);
            await _db.SaveChangesAsync();

            return dto;
        }
    }
}
