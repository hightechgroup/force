using Mapster;
using WebApp.Data;

namespace WebApp.Web.Features.WeatherForecast;

public record AddWeatherForecast(DateOnly Date, int TemperatureC, int WindSpeed, int AirHumidityPercent,
    int WeatherSummaryId) : IRequest
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
        TypeAdapterConfig<AddWeatherForecast, Domain.WeatherForecast>.NewConfig()
            .Map(x => x.SummaryId, y => y.WeatherSummaryId)
            .Map(x=>x.TemperatureF, y => 32 + (int)(y.TemperatureC / 0.5556));
        
        var forecastEntity = request.Adapt<Domain.WeatherForecast>();
        _dbContext.WeatherForecasts.Add(forecastEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}