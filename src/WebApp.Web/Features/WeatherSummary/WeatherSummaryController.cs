using WebApp.Web.Base;

namespace WebApp.Web.Features.WeatherSummary;

public class WeatherSummaryController: ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WeatherSummaryListItem>>> Get(
        [FromQuery] GetWeatherSummaryQuery request) => await Handle(request);
    
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] AddWeatherSummary request) => await Handle(request);
}