using System;
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
        public void GetConstructorInfo_FindConstructor()
        {
            var ctr = Type<TypeTestObject>.GetConstructorInfo(new object[] {1, 2});
            Assert.NotNull(ctr);
        }

        [Fact]
        public void GetConstructorInfo_ThrowsInvalidOperation()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                Type<TypeTestObject>.GetConstructorInfo(new object[] {"1", 2});
            });
        }
        
        [Fact]
        public void TryGetValue()
        {
            var productFilter = new ProductFilter(){Name = "123"};
            productFilter.TryGetValue("Name", out var val);
        }
        
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
            var setter = Type<Product>.PropertySetter<string>("Name");
            Assert.NotNull(setter);
        }
    }
}