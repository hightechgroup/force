using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using FastMember;
using Force.Demo.Web.Shop.Catalog;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Force.Demo.Web
{
    public class ImmutableModelBinder: IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var param = new TestParam(
                int.Parse(bindingContext.ValueProvider.GetValue("a").Values[0]),
                int.Parse(bindingContext.ValueProvider.GetValue("b").Values[0]));
            
            
            return Task.FromResult(param);
        }
    }
}