using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;

namespace WebApp.Web.Features.WeatherForecast;

public record AddWeatherForecast([Required] DateOnly? Date,
    [Required, Range(-100, 100)] int? TemperatureC, [Required, Range(0, 50)] int? WindSpeed,
    [Required, Range(0, 100)] int? AirHumidityPercent,
    [Required] int? WeatherSummaryId) : IRequest<int>
{
}

[UsedImplicitly]
public class AddWeatherForecastValidator : AbstractValidator<AddWeatherForecast>
{
    private readonly WebAppDbContext _dbContext;

    public AddWeatherForecastValidator(WebAppDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.WeatherSummaryId)
            .MustAsync(async (weatherSummaryId, ct) =>
            {
                return await _dbContext.WeatherSummaries
                    .AnyAsync(x => x.Id == weatherSummaryId, cancellationToken: ct);
            })
            .WithMessage("Database doesn't contain entity with given id.");

        RuleFor(x => x.Date)
            .MustAsync(async (date, ct) =>
            {
                var sameDateWeatherForecastExists = await _dbContext.WeatherForecasts
                    .AnyAsync(x => x.Date == date, cancellationToken: ct);
                return !sameDateWeatherForecastExists;
            })
            .WithMessage("The weather forecast for this date already exists.");
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
            Date = request.Date!.Value,
            TemperatureC = request.TemperatureC!.Value,
            TemperatureF = 32 + (int)(request.TemperatureC / 0.5556),
            WindSpeed = request.WindSpeed!.Value,
            AirHumidityPercent = request.AirHumidityPercent!.Value,
            SummaryId = request.WeatherSummaryId!.Value,
            Summary = null,
        };
        _dbContext.WeatherForecasts.Add(forecastEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return forecastEntity.Id;
    }
}