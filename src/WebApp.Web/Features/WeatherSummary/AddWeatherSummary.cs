using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;

namespace WebApp.Web.Features.WeatherSummary;

public record AddWeatherSummary([Required, MinLength(3)] string Summary) : IRequest<int>
{
}

[UsedImplicitly]
public class AddWeatherSummaryValidator : AbstractValidator<AddWeatherSummary>
{
    private readonly WebAppDbContext _dbContext;

    public AddWeatherSummaryValidator(WebAppDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Summary)
            .MustAsync(async (summary, ct) =>
            {
                var weatherSummaryExists = await _dbContext.WeatherSummaries
                    .AnyAsync(x => x.Summary.ToLower() == summary.ToLower(), cancellationToken: ct);
                return !weatherSummaryExists;
            })
            .WithMessage("That weather summary already exists");
    }
}

[UsedImplicitly]
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