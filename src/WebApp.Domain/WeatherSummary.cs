using System.ComponentModel.DataAnnotations;

namespace WebApp.Domain;

public class WeatherSummary
{
    public int Id { get; protected set; }
    
    [Required]
    public required string Summary { get; init; }
}