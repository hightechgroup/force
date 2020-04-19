using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using Xunit;

namespace Force.Tests.Cqrs
{
    public class FilterQueryTests: IClassFixture<ProductFixture>
    {
        private readonly ProductFixture _productFixture;

        public FilterQueryTests(ProductFixture productFixture)
        {
            _productFixture = productFixture;
        }
        
        [Fact]
        public void Sort()
        {
            var productFilter = new PagedProductFilter()
            {
                Order = "Name",
                Asc = false
            };

            var sortedExpected = _productFixture.ProductQueryable
                .OrderBy(x => x.Name)
                .ToArray();
            
            var sorted = productFilter
                .Sort(_productFixture.ProductQueryable)
                .ToArray();
            
            Assert.All(sorted, 
                x => Assert.Equal(sorted.IndexOf(x), sortedExpected.IndexOf(x)));
        }
        
        [Fact]
        public void Filter()
        {
            var productFilter = new PagedProductFilter()
            {
                Search = ProductFixture.FirstProductName,
                Asc = false
            };

            var results = productFilter
                .Filter(_productFixture.ProductQueryable)
                .ToArray();
            
            Assert.Collection(results, 
                x => Assert.Equal(ProductFixture.FirstProductName, x.Name));
        }
    }
}