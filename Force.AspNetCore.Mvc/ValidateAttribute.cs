using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Force.AspNetCore.Mvc
{
    public enum ValidationMode
    {
        Mvc,
        WebApi
    }
    
    public class ValidateAttribute: ActionFilterAttribute
    {
        private readonly ValidationMode _mode;

        public ValidateAttribute(ValidationMode mode = ValidationMode.WebApi)
        {
            _mode = mode;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                if (_mode == ValidationMode.WebApi)
                {
                    context.Result = new FailedResult(context.ModelState);
                }
                else
                {
                    // ReSharper disable once Mvc.ViewNotResolved
                    context.Result = ((Controller)context.Controller).View(context.ActionArguments.Values.First());
                    FailedResult.SetStatusCodeAndHeaders(context.HttpContext);
                }
            }
        }
    }
}