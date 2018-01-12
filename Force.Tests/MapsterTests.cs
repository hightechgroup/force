using System;
using System.Linq;
using Force.Demo.Domain;
using Mapster;
using Xunit;

namespace Force.Tests
{
    public class MapsterTests
    {
        [Fact]
        public void A()
        {
            var products = new[]
            {
                new Product("", 100),
                new Product("",200),
                new Product("",300)
            }.AsQueryable();

            var dtos = products.ProjectToType<ProductDto>().ToList();

            Assert.Equal(products.Sum(x => x.Price), dtos.Sum(x => x.Price));
        }
    }
    
    public class ProductDto
    {
        public int Id { get; set; }
                
        public decimal Price { get; set; }
    }
    
}