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
                new Product(100){Id = 1},
                new Product(200){Id = 2},
                new Product(300){Id = 3}
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