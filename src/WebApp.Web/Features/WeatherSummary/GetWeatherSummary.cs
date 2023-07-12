using System.ComponentModel;
using AutoFilterer.Extensions;
using AutoFilterer.Types;
using Mapster;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Domain;

namespace WebApp.Web.Features.WeatherSummary;

public record GetWeatherSummaryQuery(WeatherSummaryFilter? Filter): IRequest<IEnumerable<WeatherSummaryListItem>>
{
    public WeatherSummaryFilter Filter { get; init; } = Filter ?? new WeatherSummaryFilter();
}

[UsedImplicitly]
public class GetWeatherSummaryHandler: IRequestHandler<GetWeatherSummaryQuery, IEnumerable<WeatherSummaryListItem>>
{
    private readonly WebAppDbContext _dbContext;

    public GetWeatherSummaryHandler(WebAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    

    public async Task<IEnumerable<WeatherSummaryListItem>> Handle(GetWeatherSummaryQuery request,
        CancellationToken cancellationToken)
    {
        return await _dbContext.WeatherSummaries
            .ApplyFilter(request.Filter)
            .ProjectToType<WeatherSummaryListItem>()
            .ToListAsync(cancellationToken);
    }
}

public class WeatherSummaryListItem
{
    public int Id { get; set; }
    
    public string Summary { get; set; }
}

public class WeatherSummaryFilter: PaginationFilterBase
{
    public virtual int? Id { get; set; }

    public virtual string? Summary { get; set; }
}