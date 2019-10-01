using AutoMapper.QueryableExtensions;
using Demo.WebApp.Controllers;
using Demo.WebApp.Domain.Entities.Blog;
using Demo.WebApp.Features.Blog;
using Force.Cqrs;
using Force.Extensions;
using Force.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.WebApp.Features
{
    public class TestController: ApiControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromServices] DbContext dbContext, [FromQuery] FilterQuery<PostListItem> q)
        {
            
            var res = dbContext
                .Set<Post>()
                .ProjectTo<PostListItem>()
                .Filter(q)
                .PipeTo(Ok);

            return res;
        }
    }
}