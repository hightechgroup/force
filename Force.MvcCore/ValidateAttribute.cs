using System.Linq;
using Force.Demo.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Force.MvcCore
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
                    context.Result = new ValidationFailedResult(context.ModelState);
                }
                else
                {
                    context.Result = ((Controller)context.Controller).View(context.ActionArguments.Values.First());
                    ValidationFailedResult.SetStatusCodeAndHeaders(context.HttpContext);
                }
            }
        }
    }
}