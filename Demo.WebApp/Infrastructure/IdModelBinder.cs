using System.Threading.Tasks;
using Demo.WebApp.Data;
using Demo.WebApp.Domain;
using Force.Ddd;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Demo.WebApp.Infrastructure
{
    public class IdModelBinder<T>: IModelBinder
        where T : class, IHasId<int>
    {
        private readonly DemoAppDbContext _dbContext;

        public IdModelBinder(DemoAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.FieldName);
            
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
                
            }
            if (!int.TryParse(valueProviderResult.FirstValue, out var intValue))
            {
                return Task.CompletedTask;
            }
            
            if(Id<T>.TryParse(intValue, x => _dbContext.Set<T>().Find(x), out var value))
            {
                bindingContext.Result = ModelBindingResult.Success(value);          
            }

            return Task.CompletedTask;
        }
    }
}