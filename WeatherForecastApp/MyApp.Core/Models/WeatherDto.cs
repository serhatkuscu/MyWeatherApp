namespace MyApp.Core.Models
{
    public class WeatherDto
    {
        public string City { get; set; } = "";
        public decimal Temperature { get; set; }
        public string Description { get; set; } = "";
        public decimal FeelsLike { get; set; }
        public int Humidity { get; set; }
        public string WeatherIcon { get; set; } = "";
        public string? ErrorMessage { get; set; }
        public string Country { get; set; } = "";        // TR gibi ülke kodu
        public int Pressure { get; set; }                // Basınç (hPa)
        public decimal WindSpeed { get; set; }           // Rüzgâr hızı (m/s)
        public int? WindDeg { get; set; }                // Rüzgâr yönü derece
        public string? WindDirection { get; set; }       // Rüzgâr yönü (N, NE, E...)
        public DateTime? SunriseLocal { get; set; }      // Gün doğumu (yerel saat)
        public DateTime? SunsetLocal { get; set; }       // Gün batımı (yerel saat)
    }
}
