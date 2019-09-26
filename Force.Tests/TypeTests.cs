using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Force.Ddd;
using Force.Reflection;
using Force.Tests.Expressions;
using Force.Tests.Infrastructure.Context;
using Xunit;

namespace Force.Tests
{
    public class SimpeValueObject : ValueObject<string>
    {
        public SimpeValueObject(string value) : base(value)
        {
        }
    }
    
    public class TypeTests
    {
        [Fact]
        public void Create()
        {
            var vo = Type<SimpeValueObject>.CreateInstance("string");
        }

        [Fact]
        public void GetCustomAttribute()
        {
            var attr = Type<Product>.GetCustomAttribute<DisplayAttribute>();
            Assert.NotNull(attr);
        }

        [Fact]
        public void PropertySetter()
        {
            var setter = Type<string>.PropertySetter<int>("Length");
            Assert.Null(setter);
        }
    }
}