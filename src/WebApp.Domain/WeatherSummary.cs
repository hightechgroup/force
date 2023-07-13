using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApp.Domain;

public class WeatherSummary
{
    public int Id { get; protected init; }
    
    [Required]
    public required string Summary { get; init; }
}