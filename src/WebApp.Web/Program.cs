using WebApp.Data;
using WebApp.Web;

var builder = WebApplication
    .CreateBuilder(args)
    .ConfigureAppConfiguration(args)
    .ConfigureSeriLog()
    .AddDatabase<WebAppDbContext>()
    .AddHttpAccessor()
    .AddControllersAndSwagger()
    .AddMediatR()
    .AddFluentValidation()
    .AddMiddlewares();


var app = builder.Build();
app
    .UseControllers()
    .UseSwaggerAndSwaggerUI()
    .UseMiddlewares();

app.Run();