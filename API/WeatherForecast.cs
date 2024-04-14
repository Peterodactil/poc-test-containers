using System.ComponentModel.DataAnnotations.Schema;

namespace API
{
    public record WeatherForecast(int Id, DateTime Date, int TemperatureC, string? Summary)
    {
        [NotMapped]
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
