using System;
using FastMember;
using Force.Ddd;
using Force.Demo.Web;
using Force.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Force.MvcCore
{
    public static class Extensions
    {
        public static IActionResult ToActionResult<T>(this Result<T> result)
            => result.Return<IActionResult>(
                x => new OkObjectResult(x),
                x => new FailedResult(x));
    }
}