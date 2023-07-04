namespace WebApp.Web.Base;

[ApiController]
[Route("api/[controller]")]
public class ApiControllerBase : Controller
{
    protected async Task<ActionResult> Handle(
        IBaseRequest request,
        CancellationToken cancellationToken = default)
    {
        var obj = await DoHandle((dynamic)request, cancellationToken);
        return Ok(obj);
    }

    private Task DoHandle(IRequest request, CancellationToken cancellationToken) => 
        HttpContext
            .RequestServices
            .GetRequiredService<IMediator>()
            .Send(request, cancellationToken);

    private Task<TResponse> DoHandle<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken) =>
        HttpContext
            .RequestServices
            .GetRequiredService<IMediator>()
            .Send(request, cancellationToken);

    protected async Task<ActionResult<TResponse>> Handle<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse> => Ok(
        await HttpContext
            .RequestServices
            .GetRequiredService<IMediator>()
            .Send(request, cancellationToken));

    protected async Task<IActionResult> Handle<TRequest>(TRequest request,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest
    {
        await HttpContext
            .RequestServices
            .GetRequiredService<IMediator>()
            .Send(request, cancellationToken);

        return Ok();
    }
}