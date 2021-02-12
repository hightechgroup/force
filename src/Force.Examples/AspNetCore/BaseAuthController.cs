using Force.Examples.AspNetCore.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Force.Examples.AspNetCore
{
    [ApiController]
    [ApiControllerFilter]
    [Route("/api/[controller]/[action]")]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BaseAuthController : ControllerBase
    {
    }
}