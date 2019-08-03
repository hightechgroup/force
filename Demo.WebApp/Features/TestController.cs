using Demo.WebApp.Controllers;
using Force.Linq.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApp.Features
{
    public class TestController: ApiControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new PagedEnumerable<string>("string"));
        }
    }
}