using System.ComponentModel.DataAnnotations;
using System.Linq;
using Force.Extensions;
using Force.Reflection;
using Force.Tests.Expressions;
using Force.Tests.Infrastructure.Context;
using Xunit;

namespace Force.Tests.Reflection
{
    public class TypeTests
    {
        [Fact]
        public void PropertyGetter()
        {
//            var getter = Type<Product>.PropertyGetter<string>("Name");
//            var func = getter.Compile();
//            var name = func(new Product() {Name = "Product Name"});
//            Assert.Equal("Product Name", name);
        }

        [Fact]
        public void HasAttribute()
        {
            var hasDisplayAttribute = Type<ProductListItem>.HasAttribute<DisplayAttribute>();
            Assert.True(hasDisplayAttribute);

            var name = Type<ProductListItem>
                .Attributes
                .First(x => typeof(DisplayAttribute) == x.GetType())
                .PipeTo(x => (DisplayAttribute) x)
                .Name;
            
            Assert.Equal("Product List", name);
        }

        [Fact]
        public void PublicMethods()
        {
            var methods = Type<Product>.PublicMethods;
            var expectedMetods = typeof(Product)
                .GetMethods()
                .Where(x => x.IsPublic && !x.IsAbstract)
                .ToArray();

            Assert.All(methods, x => expectedMetods.Contains(x));
            Assert.All(expectedMetods, x => methods.Contains(x));
        }
    }
}