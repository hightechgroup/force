using Force.Ddd;
using Microsoft.AspNetCore.Mvc;

namespace Force.AspNetCore.Mvc
{
    public static class Extensions
    {
        public static IActionResult ToActionResult<T>(this Result<T> result)
            => result.Return<IActionResult>(
                x => new OkObjectResult(x),
                x => new FailedResult(x));
    }
}