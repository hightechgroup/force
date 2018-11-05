using Force;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApp
{
    public static class Extensions
    {
        public static IActionResult ToResult<T>(this IHandler<T> handler, T input)
        {
            handler.Handle(input);
            return new OkResult();
        }
    }
}