using WebApp.Web.Base;

namespace WebApp.Web.Features.WeatherForecast;

public class WeatherForecastController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WeatherForecastListItem>>> Get(
        [FromQuery] GetWeatherForecastQuery request) => await Handle(request);
    
    [HttpPost]
    public async Task<ActionResult<int>> Create(
        [FromBody] AddWeatherForecast request) => await Handle(request);
}