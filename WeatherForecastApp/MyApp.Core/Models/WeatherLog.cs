namespace MyApp.Core.Models
{
    public class WeatherLog
    {
        public int Id { get; set; }              // Primary Key
        public string City { get; set; } = "";
        public decimal Temperature { get; set; }
        public DateTime CreatedAt { get; set; }  // Log’un tutulduğu an
    }
}
