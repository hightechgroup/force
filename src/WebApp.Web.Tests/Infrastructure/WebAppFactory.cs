using AspNetCore.Testing.MoqWebApplicationFactory;
using WebApp.Web.Features.WeatherForecast;

namespace WebApp.Web.Tests.Infrastructure;

public class WebAppFactory : MoqWebApplicationFactory<WeatherForecastController> { }