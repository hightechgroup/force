using WebApp.Data;
using WebApp.Web;

var builder = WebApplication
    .CreateBuilder(args)
    .ConfigureAppConfiguration(args)
    .AddDatabase<WebAppDbContext>()
    .AddHttpAccessor()
    .AddControllersAndSwagger()
    .AddMediatR();

var app = builder.Build();
app
    .UseControllers()
    .UseSwaggerAndSwaggerUI();

app.Run();