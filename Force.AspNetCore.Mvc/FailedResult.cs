using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Force.AspNetCore.Mvc
{
    public class FailedResult: IActionResult
    {
        public FailedResult(ModelStateDictionary modelState)
        {
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}