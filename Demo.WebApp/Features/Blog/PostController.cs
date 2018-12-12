using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Demo.WebApp.Controllers;
using Demo.WebApp.Domain;
using Force.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.WebApp.Features.Blog
{
    public class PostController: ApiControllerBase
    {
        private readonly IQueryable<Post> _posts;

        public PostController(IQueryable<Post> posts)
        {
            _posts = posts;
        }

        [HttpGet]
        public async Task<ActionResult<int>> Get([FromQuery]PostListQuery query)
            => (await _posts
                    .ProjectTo<PostListDto>()
                    .FilterAndSort(query)
                    .ToListAsync())
                    .PipeTo(Ok);      
    }
}