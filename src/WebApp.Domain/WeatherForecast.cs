using System.ComponentModel.DataAnnotations;

namespace WebApp.Domain;

public class WeatherForecast
{
    public int Id { get; protected set; }
    
    [Required]
    public required string Summary { get; init; }
}