namespace Force.Web.Features.WeatherForecast.GetWeatherForecast;

[UsedImplicitly]
public class GetWeatherForecastHandler: IRequestHandler<GetWeatherForecastQuery, IEnumerable<WeatherForecastListItem>>
{
    private static readonly string[] Summaries = {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    
    public Task<IEnumerable<WeatherForecastListItem>> Handle(GetWeatherForecastQuery request, 
        CancellationToken cancellationToken) =>
        Enumerable
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