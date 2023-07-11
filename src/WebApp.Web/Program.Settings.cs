using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApp.Web.Base;

namespace WebApp.Web;

internal static class Settings
{
    public static WebApplicationBuilder AddDatabase<T>(this WebApplicationBuilder builder)
        where T:DbContext
    {
        builder.Services.AddDbContext<T>(options =>
            {
                //options.UseInMemoryDatabase("InMemory");
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly(typeof(T).Assembly.ToString()));
            }
        );

        return builder;
    }
    
    public static WebApplicationBuilder AddHttpAccessor(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddScoped(x => x.GetRequiredService<IHttpContextAccessor>().HttpContext?.User.Identity 
                                        ?? throw new InvalidOperationException("HttpContext is null"));

        return builder;
    }
    
    public static WebApplicationBuilder ConfigureAppConfiguration(this WebApplicationBuilder builder, string[] strings)
    {
        builder.Configuration
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{builder.Configuration["Environment"]}.json", false)
            .AddCommandLine(strings)
            .AddEnvironmentVariables();

        return builder;
    }

    public static WebApplicationBuilder AddControllersAndSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(options =>
        {
           // TODO: is not supported in test code 
            // options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
        });
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
        return builder;
    }

    public static WebApplicationBuilder AddMediatR(this WebApplicationBuilder builder)
    {
        var type = typeof(Settings);
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(type));
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        return builder;
    }
    
    // ReSharper disable once UnusedMethodReturnValue.Global
    // ReSharper disable once InconsistentNaming
    public static WebApplication UseSwaggerAndSwaggerUI(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }

    public static WebApplication UseControllers(this WebApplication app)
    {
        app.MapControllers();

        return app;
    }
}