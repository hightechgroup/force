using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNext.DddWorkshop.Infrastructure
{
    public class ValidationFilter: IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var c = ((Controller) context.Controller);
                c.ViewData["Message"] = "Model is invalid!";
                context.Result = c.View(context.ActionArguments.Values.First());
            }
        }
    }
}