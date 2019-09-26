using Demo.WebApp.Infrastructure;
using Force.Linq.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApp.Features
{
    public class TestController: ApiControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}