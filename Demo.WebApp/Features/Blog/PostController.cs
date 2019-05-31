using System;
using System.Collections.Generic;
using System.Linq;
using Demo.WebApp.Controllers;
using Demo.WebApp.Data;
using Demo.WebApp.Domain;
using Force.AspNetCore.Mvc;
using Force.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Demo.WebApp.Features.Blog
{
    public class PostController: ApiControllerBase
    {
        //[HttpGet]
        public ActionResult<IEnumerable<PostListDto>> Get([FromQuery] PostListQuery query)
            => this.FetchEnumerable(query);
        
        static readonly Func<DbContext, IEnumerable<PostListDto>> Compiled = EF.CompileQuery(
            (DbContext db) => db
                .Set<Post>()
                .Select(x => new PostListDto()
                {
                    Id = x.Id, 
                    Title = x.Name
                }));

        [HttpGet]
        public ActionResult<IEnumerable<PostListDto>> Get([FromServices] DbContext dbContext)
            => Compiled.Invoke(dbContext).PipeTo(Ok);

    }
}