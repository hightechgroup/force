using System;
using System.Linq;
using System.Threading.Tasks;
using FastMember;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Force.AspNetCore.Mvc
{
    public class EntityModelBinder: IModelBinder
    {
        private readonly Func<Type, object, object> _getter;

        public EntityModelBinder(Func<Type, object, object> getter)
        {
            _getter = getter;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ActionContext
                .HttpContext
                .Request
                .Query[bindingContext.ModelName]
                .FirstOrDefault();

            if (value == null)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Id for \"bindingContext.ModelName\" is null");
                return Task.CompletedTask;
            }

            try
            {
                var typeAccessor = TypeAccessor.Create(bindingContext.ModelType);
                var id = Convert.ChangeType(value, typeAccessor.GetMembers().First(y => y.Name == "Id").Type);
                var result = _getter(bindingContext.ModelType, id);
                bindingContext.Result = ModelBindingResult.Success(result);
            }
            catch (Exception e)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, e.Message);
            }
            
            return Task.CompletedTask;
        }
    }
    
}