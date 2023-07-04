using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Data;

public class WebAppDbContext: DbContext
{
    public DbSet<WeatherForecast> WeatherForecasts => Set<WeatherForecast>();
    
    public WebAppDbContext(DbContextOptions<WebAppDbContext> options) : base(options) { }
}

public class WeatherForecast
{
    public int Id { get; protected set; }
    
    [Required]
    public required string Summary { get; init; }
}