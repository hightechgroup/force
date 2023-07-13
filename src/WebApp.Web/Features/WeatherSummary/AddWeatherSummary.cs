using Mapster;
using WebApp.Data;

namespace WebApp.Web.Features.WeatherSummary;

public record AddWeatherSummary(string Summary): IRequest<int>
{
    
}

public class AddWeatherSummaryHandler : IRequestHandler<AddWeatherSummary, int>
{
    private readonly WebAppDbContext _dbContext;

    public AddWeatherSummaryHandler(WebAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> Handle(AddWeatherSummary request, CancellationToken cancellationToken)
    {
        var summaryEntity = new Domain.WeatherSummary()
        {
            Summary = request.Summary,
        };
        _dbContext.WeatherSummaries.Add(summaryEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return summaryEntity.Id;
    }
}