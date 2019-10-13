using Force.Cqrs;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;

namespace WebApplication.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult ToViewOrNotFound(this object obj, Controller controller)
            => obj != null
                ? (IActionResult)controller.View(obj)
                : new NotFoundResult();

        public static QueryResultViewModel<T> ToQueryResultViewModel<T>(this T result, IQuery<T> query)
            => new QueryResultViewModel<T>(query, result);
    }
}