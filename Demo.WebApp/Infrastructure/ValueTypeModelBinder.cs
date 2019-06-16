using System.Threading.Tasks;
using Demo.WebApp.Domain;
using Force.Ddd;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Demo.WebApp.Infrastructure
{
    public class ValueTypeModelBinder: IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var fieldName = bindingContext.ModelName;
            var valueProviderResult = bindingContext.ValueProvider.GetValue(fieldName);
            
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
                
            }
            
            if (ValueObject.TryParse(valueProviderResult.FirstValue, out var value))
            {
                bindingContext.Result = ModelBindingResult.Success(value);
            }
            
            return Task.CompletedTask;
        }
    }
}