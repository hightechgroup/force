using System.ComponentModel.DataAnnotations;
using Force.Reflection;
using Force.Tests.Expressions;
using Xunit;

namespace Force.Tests.Reflection
{
    public class TypeTests
    {
        [Fact]
        public void PropertyGetter()
        {
            var getter = Type<Product>.PropertyGetter<string>("Name");
            var func = getter.Compile();
            var name = func(new Product() {Name = "John"});
            Assert.Equal("John", name);
        }

        [Fact]
        public void HasAttribute()
        {
            Type<ProductListItem>.HasAttribute<DisplayAttribute>();
        }

        [Fact]
        public void PublicMethods()
        {
            var methods = Type<Product>.PublicMethods;
        }
    }
}