using Microsoft.EntityFrameworkCore;
using WebApp.Data;

namespace WebApp.Web.Features.WeatherForecast;

public record GetWeatherForecastQuery([Range(0,5)]int Skip): IRequest<IEnumerable<WeatherForecastListItem>>;

[UsedImplicitly]
public class GetWeatherForecastHandler: IRequestHandler<GetWeatherForecastQuery, IEnumerable<WeatherForecastListItem>>
{
    private readonly DbContext _dbContext;

    private static readonly string[] Summaries = {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public GetWeatherForecastHandler(WebAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    

    public Task<IEnumerable<WeatherForecastListItem>> Handle(GetWeatherForecastQuery request,
        CancellationToken cancellationToken)
    {
        _dbContext.Set<Domain.WeatherForecast>().Add(new Domain.WeatherForecast() { Summary = Summaries[0] });
        _dbContext.SaveChanges();

        var fc = _dbContext.Set<Domain.WeatherForecast>().ToList();
        
        return Enumerable
            .Range(1, 5)
            .Skip(request.Skip)
            .Select(index => new WeatherForecastListItem
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray()
            .Cast<WeatherForecastListItem>()
            .PipeTo(Task.FromResult);
    }

}

public class WeatherForecastListItem
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}