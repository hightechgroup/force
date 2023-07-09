using WebApp.Web.Tests.Infrastructure;

namespace WebApp.Web.Tests.WeatherForecast;

public class WeatherForecastControllerWafTests: WeatherForecastControllerTestsBase<WebAppMoqHttpClientFactory>,
    IClassFixture<WebAppMoqHttpClientFactory>
{
    public WeatherForecastControllerWafTests(WebAppMoqHttpClientFactory http) : base(http) { }
}