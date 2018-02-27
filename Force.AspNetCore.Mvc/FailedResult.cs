using System.Linq;
using Force.Ddd;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Force.AspNetCore.Mvc
{
    // https://stackoverflow.com/questions/3290182/rest-http-status-codes-for-failed-validation-or-invalid-duplicate
    internal class FailedResult: JsonResult
    {
        public FailedResult(ModelStateDictionary modelState)
            : base(modelState.Select(x => new
                {
                    x.Key,
                    ValidationState = x.Value.ValidationState.ToString(),
                    x.Value.Errors
                }).ToList())        
        {
        }

        public FailedResult(Failure failure)
            : base(failure)
        {         
        }

        public override void ExecuteResult(ActionContext context)
        {
            base.ExecuteResult(context);
            SetStatusCodeAndHeaders(context.HttpContext);
        }

        internal static void SetStatusCodeAndHeaders(HttpContext context)
        {
            context.Response.StatusCode = 422;
            context.Response.Headers.Add("X-Status-Reason", "Validation failed");
        }
    }
}