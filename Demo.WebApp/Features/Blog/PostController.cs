using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.WebApp.Controllers;
using Demo.WebApp.Infrastructure;
using Force.Cqrs;
using Force.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApp.Features.Blog
{
    public class PostController: ApiControllerBase
    {
        private readonly IQueryHandler<PostListQuery, Task<IEnumerable<PostListDto>>> _postListQueryHandler;

        public PostController(IQueryHandler<PostListQuery, Task<IEnumerable<PostListDto>>> postListQueryHandler)
        {
            _postListQueryHandler = postListQueryHandler;
        }

        [HttpGet]
        public async Task<ActionResult<int>> Get([FromQuery] PostListQuery query)
            => await _postListQueryHandler
                .Handle(query)
                .PipeToAsync(Ok);
    }
}