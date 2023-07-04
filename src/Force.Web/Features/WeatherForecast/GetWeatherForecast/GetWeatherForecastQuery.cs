using System.ComponentModel.DataAnnotations;

namespace Force.Web.Features.WeatherForecast.GetWeatherForecast;

public record GetWeatherForecastQuery([Range(0,5)]int Skip): IRequest<IEnumerable<WeatherForecastListItem>>;