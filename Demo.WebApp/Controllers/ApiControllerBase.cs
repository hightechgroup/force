using Force.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApp.Controllers
{
    [Route("api/[controller]")]
    [Validate]
    [ApiController]
    public abstract class ApiControllerBase: ControllerBase
    {
    }
}