using System.Text.Json;

namespace WebApp.Web.Base;

public class ExceptionHandlingMiddleware: IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (CustomValidationException e)
        {
            await HandleCustomValidationException(context, e);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    private async Task HandleCustomValidationException(HttpContext httpContext, CustomValidationException exception)
    {
        var response = new
        {
            type = "https://tools.ietf.org/html/rfc9110#section-15.5.21",
            title = "One or more validation errors occurred.",
            status = StatusCodes.Status422UnprocessableEntity,
            errors = exception.ValidationMessages,
            traceId = httpContext.TraceIdentifier,
        };
        
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var response = new
        {
            title = "Server Error",
            status = StatusCodes.Status500InternalServerError,
            errors = exception.Message
        };
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
   
}