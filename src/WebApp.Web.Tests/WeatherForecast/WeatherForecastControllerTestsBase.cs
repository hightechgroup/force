using AspNetCore.Testing.MoqWebApplicationFactory;
using WebApp.Web.Features.WeatherForecast;

namespace WebApp.Web.Tests.WeatherForecast;

public abstract class WeatherForecastControllerTestsBase<T>: ControllerTestsBase<WeatherForecastController, T>
    where T : IHttpClientFactory
{
    protected WeatherForecastControllerTestsBase(T http) : base(http) { }

    [Fact]
    public async Task Test1()
    {
        // var client = CreateControllerClient();
        // var request = new GetWeatherForecastQuery(0);
        // var response = await client.SendAsync(c => c.Get(request));
        // Assert.NotNull(response);
    }
}