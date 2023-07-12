using WebApp.Data;
using WebApp.Web.Features.WeatherSummary;

namespace WebApp.Web.Features.WeatherForecast;

public record AddWeatherForecast(AddWeatherForecastModel Model): IRequest
{
    
}

public class AddWeatherForecastHandler : IRequestHandler<AddWeatherForecast>
{
    private readonly WebAppDbContext _dbContext;

    public AddWeatherForecastHandler(WebAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(AddWeatherForecast request, CancellationToken cancellationToken)
    {
        var forecastEntity = new Domain.WeatherForecast
        {
            Date = request.Model.Date,
            TemperatureC = request.Model.TemperatureC,
            AirHumidityPercent = request.Model.AirHumidityPercent,
            WindSpeed = request.Model.WindSpeed,
            TemperatureF = 32 + (int)(request.Model.TemperatureC / 0.5556),
            SummaryId = request.Model.WeatherSummaryId,
            Summary = null,
        };

        _dbContext.WeatherForecasts.Add(forecastEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

public class AddWeatherForecastModel
{
    public DateOnly Date { get; set; }
    
    public int TemperatureC { get; set; }
    
    public int WindSpeed { get; set; }
    
    public int AirHumidityPercent { get; set; }
    
    public int WeatherSummaryId { get; set; }
}