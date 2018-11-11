using System.Linq;
using Force.Ddd;
using Xunit;

namespace Force.Tests
{
    public class AutoFilterTests: DbContextTestsBase
    {
        [Fact]
        public void Filter()
        {
//            var filter = new AutoFilter<Product>(new ProductFilter()
//            {
//                Name = "1"
//            });
//            
//            var products = filter.Filter(DbContext.Products).ToList();
//            
//            Assert.Single(products);
//            Assert.Equal("1", products.First().Name);
        }
    }
}