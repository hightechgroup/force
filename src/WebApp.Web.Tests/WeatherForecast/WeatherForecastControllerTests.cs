using AspNetCore.Testing.MoqWebApplicationFactory;
using WebApp.Data;
using WebApp.Web.Features.WeatherForecast;
using WebApp.Web.Tests.Infrastructure;

namespace WebApp.Web.Tests.WeatherForecast;

public class WeatherForecastControllerTests: 
    WeatherForecastControllerTestsBase<MoqHttpClientFactory<WeatherForecastController>>,
    IClassFixture<MoqHttpClientFactory<WeatherForecastController>>
{
    public WeatherForecastControllerTests(MoqHttpClientFactory<WeatherForecastController> httpClientFactory) : base(httpClientFactory) { }
}