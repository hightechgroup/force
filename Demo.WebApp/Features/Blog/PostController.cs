using System;
using System.Collections.Generic;
using System.Linq;
using Demo.WebApp.Controllers;
using Demo.WebApp.Data;
using Demo.WebApp.Domain;
using Demo.WebApp.Infrastructure;
using Force.AspNetCore.Mvc;
using Force.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Demo.WebApp.Features.Blog
{
    public class PostController: ApiControllerBase
    {
       [HttpGet]
        public ActionResult<IEnumerable<PostListDto>> Get([FromQuery] PostListQuery query)
            => this.FetchEnumerable(query);
    }
}