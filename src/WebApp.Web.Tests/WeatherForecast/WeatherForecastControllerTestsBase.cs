using System.Net;
using AspNetCore.Testing.Expressions;
using AspNetCore.Testing.MoqWebApplicationFactory;
using Microsoft.AspNetCore.Mvc;
using WebApp.Web.Features.WeatherForecast;
using WebApp.Web.Features.WeatherSummary;

namespace WebApp.Web.Tests.WeatherForecast;

public abstract class WeatherForecastControllerTestsBase<T> : ControllerTestsBase<WeatherForecastController, T>
    where T : IHttpClientFactory
{
    protected WeatherForecastControllerTestsBase(T http) : base(http)
    {
    }

    [Fact]
    public async Task Create_InvalidInputData_BadRequestStatusCode()
    {
        //arrange
        var client = CreateControllerClient();
        var request = new AddWeatherForecast(null, -12352, 1242, 146, -125);

        try
        {
            //act
            var response = await client.SendAsync(c => c.Create(request));
            Assert.Fail("Weather forecast created on invalid data");
        }
        catch (HttpRequestException ex)
        {
            //assert
            Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);
        }
    }

    [Fact]
    public async Task Create_ValidInputData_ForecastCreated()
    {
        //arrange
        var summaryClient = new ControllerClient<WeatherSummaryController>(CreateHttpClient());
        var forecastClient = CreateControllerClient();

        var newSummaryName = Guid.NewGuid().ToString();
        var newSummaryId = await summaryClient
            .SendAsync(x => x.Create(new AddWeatherSummary(newSummaryName)));

        var existForecasts = await forecastClient
            .SendAsync(x => x.Get(new GetWeatherForecastQuery(null)));
        var lastDate = existForecasts
            .Select(x => x.Date)
            .OrderByDescending(x => x)
            .FirstOrDefault();

        var newForecast = new AddWeatherForecast(TemperatureC: 21, WindSpeed: 7, AirHumidityPercent: 70,
            WeatherSummaryId: newSummaryId, Date: lastDate.AddDays(1));
        
        //act
        var response = await forecastClient.SendAsync(c => c.Create(newForecast));
        
        //assert
        Assert.NotEqual(0, response);
    }
}