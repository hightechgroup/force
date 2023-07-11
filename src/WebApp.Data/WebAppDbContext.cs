using Microsoft.EntityFrameworkCore;
using WebApp.Domain;

namespace WebApp.Data;

public class WebAppDbContext: DbContext
{
    public DbSet<WeatherForecast> WeatherForecasts => Set<WeatherForecast>();

    public WebAppDbContext(DbContextOptions<WebAppDbContext> options) : base(options)
    {
        if (Database.EnsureCreated())
        {
            Console.WriteLine("log");
        }
    }
}