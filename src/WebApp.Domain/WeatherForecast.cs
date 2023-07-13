using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApp.Domain;

public class WeatherForecast
{
    public int Id { get; protected init; }
    
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
    
    [SetsRequiredMembers]
    public WeatherForecast(DateOnly date, int temperatureC, int windSpeed, int airHumidityPercent, int summaryId)
    {
        Date = date;
        TemperatureC = temperatureC;
        TemperatureF = 32 + (int)(temperatureC / 0.5556);
        WindSpeed = windSpeed;
        AirHumidityPercent = airHumidityPercent;
        SummaryId = summaryId;
    }

    private WeatherForecast(){}
}