namespace Force.Web.Features.WeatherForecast.GetWeatherForecast;

public class WeatherForecastListItem
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}