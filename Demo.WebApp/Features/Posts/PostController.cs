using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Demo.WebApp.Controllers;
using Demo.WebApp.Data;
using Demo.WebApp.Domain;
using Force.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApp.Features.Posts
{
    public class PostController: ApiControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromServices] DemoAppDbContext dbContext, [FromQuery] PostListQuery query)
            => dbContext
                .Set<Post>()
                .Where(CanBePublishedSpec<Post>.Published)
                .ProjectTo<PostListDto>()
                .FilterAndSort(query)
                .ToList()
                .PipeTo(Ok);

        [HttpPost("import")]
        public IActionResult Import([FromBody]ImportPost[] posts)
        {
            return Ok(new
            {
                posts,
                ModelState = ModelState.IsValid
            });
        }
      
    }
}