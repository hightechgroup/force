using Mapster;
using WebApp.Data;

namespace WebApp.Web.Features.WeatherSummary;

public record AddWeatherSummary(AddWeatherSummaryModel model): IRequest
{
    
}

public class AddWeatherSummaryHandler : IRequestHandler<AddWeatherSummary>
{
    private readonly WebAppDbContext _dbContext;

    public AddWeatherSummaryHandler(WebAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(AddWeatherSummary request, CancellationToken cancellationToken)
    {
        var summaryEntity = request.model.Adapt<Domain.WeatherSummary>();
        _dbContext.WeatherSummaries.Add(summaryEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

public class AddWeatherSummaryModel
{
    public string Summary { get; set; }
}