using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Demo.WebApp.Controllers;
using Demo.WebApp.Data;
using Demo.WebApp.Domain;
using Demo.WebApp.Infrastructure;
using Force.AutoMapper;
using Force.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApp.Features.Posts
{
    public class PostController: ApiControllerBase
    {
        [HttpGet]
        public IActionResult Get(
            [FromServices] LinqQueryHandler<PostListQuery, Post, PostListDto> linqQueryHandler,
            [FromQuery] PostListQuery query)
            => linqQueryHandler.ToResult(query);

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