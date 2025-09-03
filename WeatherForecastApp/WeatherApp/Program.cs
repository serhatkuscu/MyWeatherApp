using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Services;
using WeatherApp.Data;

class Program
{
    static async Task Main()
    {
        // 1. Service Collection oluştur
        var services = new ServiceCollection();

        // 2. DbContext ekle (SQL Server örneği)
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer("Server=.;Database=WeatherAppDb;Trusted_Connection=True;TrustServerCertificate=True;"));

        // 3. WeatherService ekle
        services.AddTransient<WeatherService>();

        // 4. ServiceProvider oluştur
        var serviceProvider = services.BuildServiceProvider();

        // 5. WeatherService kullan
        var service = serviceProvider.GetRequiredService<WeatherService>();

        Console.Write("Şehir adı gir: ");
        var city = Console.ReadLine();

        var result = await service.GetWeatherAsync(city);

        if (result != null)
        {
            Console.WriteLine($"{result.City}: {result.Temperature}°C, {result.Description}");
        }
        else
        {
            Console.WriteLine("Hava durumu alınamadı.");
        }
    }
}
