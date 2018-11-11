using System;
using System.Linq;
using Force.Ddd;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Demo.WebApp.Infrastructure
{
    public class IdModelBinderProvider: IModelBinderProvider
         {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType.IsGenericType
                && context.Metadata.ModelType.GetGenericTypeDefinition() == typeof(Id<>))
            {
                return new BinderTypeModelBinder(
                    typeof(IdModelBinder<>).MakeGenericType(context.Metadata.ModelType.GenericTypeArguments.First()));
            }

            return null;
        }    
    }
}