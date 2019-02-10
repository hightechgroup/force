using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Demo.WebApp.Controllers;
using Demo.WebApp.Features.Import;
using Demo.WebApp.Infrastructure;
using Force;
using Force.Cqrs;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApp.Features.Blog
{
    public class PostController: ApiControllerBase
    {
        [HttpGet]
        public IActionResult Get(
            [FromServices] IQueryHandler<PostListQuery, IEnumerable<PostListDto>> linqQueryHandlerpostListHandler,
            [FromQuery] PostListQuery query)
            => linqQueryHandlerpostListHandler.ToResult(query);

        [HttpGet("import")]
        public IActionResult Import(
            [FromServices] IHandler<IEnumerable<ImportPost>, IEnumerable<ValidationResult>> handler)
        {
            var res = handler.Handle(new List<ImportPost>());
            return Ok(res);

        }
      
    }
}