using AutoFilterer.Extensions;
using AutoFilterer.Types;
using Mapster;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
namespace WebApp.Web.Features.WeatherForecast;

public record GetWeatherForecastQuery(WeatherForecastFilter? Filter) : IRequest<IEnumerable<WeatherForecastListItem>>
{
    public required WeatherForecastFilter Filter { get; init; } = Filter ?? new WeatherForecastFilter();
}


[UsedImplicitly]
public class GetWeatherForecastHandler: IRequestHandler<GetWeatherForecastQuery, IEnumerable<WeatherForecastListItem>>
{
    private readonly WebAppDbContext _dbContext;

    public GetWeatherForecastHandler(WebAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    

    public async Task<IEnumerable<WeatherForecastListItem>> Handle(GetWeatherForecastQuery request,
        CancellationToken cancellationToken)
    {
        TypeAdapterConfig<Domain.WeatherForecast, WeatherForecastListItem>.NewConfig()
            .Map(x => x.Summary, y=>y.Summary.Summary);
        
        return await _dbContext.WeatherForecasts
            .ApplyFilter(request.Filter)
            .ProjectToType<WeatherForecastListItem>()
            .ToListAsync(cancellationToken);
    }

}

public class WeatherForecastListItem
{
    public int Id { get; set; }
    
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string Summary { get; set; }
}

public class WeatherForecastFilter : PaginationFilterBase
{
    public virtual int? Id { get; set; }
    
    public virtual DateOnly? Date { get; set; }
    
    public virtual Range<int>? TemperatureC { get; set; }
    
    public virtual Range<int>? TemperatureF { get; set; }
    
    public virtual string? Summary { get; set; }
}