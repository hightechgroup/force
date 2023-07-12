using Mapster;
using WebApp.Data;

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
        TypeAdapterConfig<AddWeatherForecastModel, Domain.WeatherForecast>.NewConfig()
            .Map(x => x.SummaryId, y => y.WeatherSummaryId)
            .Map(x=>x.TemperatureF, y => 32 + (int)(y.TemperatureC / 0.5556));
        
        var forecastEntity = request.Model.Adapt<Domain.WeatherForecast>();
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