using AspNetCore.Testing.MoqWebApplicationFactory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Data;
using WebApp.Web.Features.WeatherForecast;

namespace WebApp.Web.Tests.Infrastructure;

public class WebAppFactory : MoqWebApplicationFactory<WeatherForecastController>
{

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureServices(services =>
        {
            var dbName = Guid.NewGuid().ToString();
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<WebAppDbContext>));

            services.Remove(descriptor);

            services.AddDbContext<WebAppDbContext>(options =>
            {
                options.UseInMemoryDatabase(dbName);
            });
        });
    }
}