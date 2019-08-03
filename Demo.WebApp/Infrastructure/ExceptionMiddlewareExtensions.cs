using System.Net;
using Castle.Core.Logging;
using Force;
using Force.Ddd;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Demo.WebApp.Infrastructure
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
 
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature != null)
                    { 
                        logger.Error($"Something went wrong: {contextFeature.Error}");
 
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new 
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = (contextFeature.Error as IHasUserFrendlyMessage)?.Message ?? "Internal Server Error."
                        }));
                    }
                });
            });
        }
    }
}