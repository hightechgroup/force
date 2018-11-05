using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiControllerBase: ControllerBase
    {        
    }
}