var builder = WebApplication
    .CreateBuilder(args)
    .ConfigureAppConfiguration(args)
    .AddHttpAccessor()
    .AddControllersAndSwagger()
    .AddMediatR();


var app = builder.Build();
app
    .UseControllers()
    .UseSwaggerAndSwaggerUI();


app.Run();