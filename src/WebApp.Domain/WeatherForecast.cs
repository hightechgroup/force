using System.ComponentModel.DataAnnotations;

namespace WebApp.Domain;

public class WeatherForecast
{
    public int Id { get; protected set; }
    
    [Required]
    public required DateOnly Date { get; init; }
    
    [Required]
    public required int TemperatureC { get; init; }
    
    [Required]
    public required int TemperatureF { get; init; }
    
    [Required]
    public required double WindSpeed { get; init; }
    
    [Required]
    public required int AirHumidityPercent { get; init; }
    
    [Required]
    public required int SummaryId { get; init; }
    
    public virtual required WeatherSummary Summary { get; init; }
}