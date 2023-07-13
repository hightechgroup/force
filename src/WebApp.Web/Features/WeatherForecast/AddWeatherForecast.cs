using FluentValidation;
using Mapster;
using WebApp.Data;

namespace WebApp.Web.Features.WeatherForecast;

public record AddWeatherForecast(DateOnly Date, int TemperatureC, int WindSpeed, int AirHumidityPercent,
    int WeatherSummaryId) : IRequest<int>
{
}

[UsedImplicitly]
public class AddWeatherForecastValidator : AbstractValidator<AddWeatherForecast>
{
    public AddWeatherForecastValidator()
    {
        RuleFor(x => x.TemperatureC)
            .NotEmpty()
            .GreaterThan(100)
            .LessThan(100);

        RuleFor(x => x.AirHumidityPercent)
            .NotEmpty()
            .GreaterThan(0)
            .LessThan(100);
        
        RuleFor(x => x.WindSpeed)
            .NotEmpty()
            .GreaterThan(0)
            .LessThan(50);

        RuleFor(x => x.Date)
            .NotEmpty();
        
        RuleFor(x => x.WeatherSummaryId)
            .NotEmpty();
    }
}

[UsedImplicitly]
public class AddWeatherForecastHandler : IRequestHandler<AddWeatherForecast, int>
{
    private readonly WebAppDbContext _dbContext;


    public AddWeatherForecastHandler(WebAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> Handle(AddWeatherForecast request, CancellationToken cancellationToken)
    {
        var forecastEntity = new Domain.WeatherForecast()
        {
            Date = request.Date,
            TemperatureC = request.TemperatureC,
            TemperatureF = 32 + (int)(request.TemperatureC / 0.5556),
            WindSpeed = request.WindSpeed,
            AirHumidityPercent = request.AirHumidityPercent,
            SummaryId = request.WeatherSummaryId,
            Summary = null,
        };
        _dbContext.WeatherForecasts.Add(forecastEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return forecastEntity.Id;
    }
}