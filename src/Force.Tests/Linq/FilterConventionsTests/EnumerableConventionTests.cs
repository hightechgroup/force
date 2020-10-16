using System.Linq;
using Force.Cqrs;
using Force.Linq;
using Force.Tests.Infrastructure.Context;
using Xunit;

namespace Force.Tests.Linq
{
    public class EnumerableConventionTests : DbContextFixtureTestsBase
    {
        public EnumerableConventionTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
        {
        }

        [Fact]
        public void Test_Filter()
        {
            var predicate = new ProductListItemFilter()
            {
                CategoryName = new []{"C1"}
            };

            var products = DbContext
                .Products
                .Select(x => new ProductListItem()
                {
                    Name = x.Name,
                    CategoryName = x.Category.Name
                });
            
            var actualFilteredProducts = products
                .Filter(predicate)
                .ToList();

            var expectedFilteredProducts = products
                .Where(x => x.CategoryName == "C1")
                .ToList();

            Assert.DoesNotContain(actualFilteredProducts, x => x.CategoryName != "C1");
            Assert.Equal(actualFilteredProducts.Count, expectedFilteredProducts.Count);
        }
    }

    internal class ProductListItemFilter : FilterQuery<ProductListItem>
    {
        public string[] CategoryName { get; set; }
    }
}