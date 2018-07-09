using System;
using System.Linq.Expressions;
using Force.Extensions;
using Xunit;

namespace Force.Tests
{
    public class ExpressionTests
    {
        [Fact]
        public void AsFunc_Caches()
        {
            Expression<Func<Product, bool>> price1 = x => x.Price > 10;
            Expression<Func<Product, bool>> price2 = x => x.Price > 10;
            Expression<Func<Product, bool>> price3 = x => x.Price > 15;

            var func1 = price1.AsFunc();
            var func2 = price2.AsFunc();
            var func3 = price3.AsFunc();
            
            Assert.Equal(func1, func2);
            Assert.NotEqual(func1, func3);
        }
    }

    public class Product
    {
        public int Id { get; set; }
        
        public decimal Price { get; set; }
    }
}