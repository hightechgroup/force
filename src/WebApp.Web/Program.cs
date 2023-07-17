using WebApp.Data;
using WebApp.Web;
using WebApp.Web.Base;

var builder = WebApplication
    .CreateBuilder(args)
    .ConfigureAppConfiguration(args)
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