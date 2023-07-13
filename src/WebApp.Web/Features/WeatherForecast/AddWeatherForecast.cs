using Mapster;
using WebApp.Data;

namespace WebApp.Web.Features.WeatherForecast;

public record AddWeatherForecast(DateOnly Date, int TemperatureC, int WindSpeed, int AirHumidityPercent,
    int WeatherSummaryId) : IRequest<int>
{
}

public class AddWeatherForecastHandler : IRequestHandler<AddWeatherForecast, int>
{
    private readonly WebAppDbContext _dbContext;


    public AddWeatherForecastHandler(WebAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> Handle(AddWeatherForecast request, CancellationToken cancellationToken)
    {
        var forecastEntity = new Domain.WeatherForecast(request.Date, request.TemperatureC, request.WindSpeed,
            request.AirHumidityPercent, request.WeatherSummaryId);
        _dbContext.WeatherForecasts.Add(forecastEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return forecastEntity.Id;
    }
}