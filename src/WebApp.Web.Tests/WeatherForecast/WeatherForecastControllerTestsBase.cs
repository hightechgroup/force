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
        var client = CreateControllerClient(); // throws exception. Perhaps because of .NET 6/.NET 8 mismatch
        var response = await client.SendAsync(c => c.Get(new GetWeatherForecastQuery(0)));
        Assert.NotNull(response);
    }
}