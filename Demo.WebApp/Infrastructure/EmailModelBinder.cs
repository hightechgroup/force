using System.Threading.Tasks;
using Demo.WebApp.Domain;
using Demo.WebApp.Domain.Entities.Account;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Demo.WebApp.Infrastructure
{
    public class EmailModelBinder: IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue("email");
            
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
                
            }
            
            if (Email.TryParse(valueProviderResult.FirstValue, out var email))
            {
                bindingContext.Result = ModelBindingResult.Success(email);
            }
            
            return Task.CompletedTask;
        }
    }
}