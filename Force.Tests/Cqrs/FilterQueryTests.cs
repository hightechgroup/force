using System.Linq;
using Force.Tests.Infrastructure;
using Force.Tests.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Internal;
using Xunit;

namespace Force.Tests.Cqrs
{
    public class FilterQueryTests: DbContextFixtureTestsBase
    {
        public FilterQueryTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
        {
        }
        
        [Fact]
        public void Sort()
        {
            var productFilter = new PagedProductFilter()
            {
                Order = "Name",
                Asc = false
            };

            var sortedExpected = DbContext
                .Products
                .OrderBy(x => x.Name)
                .ToArray();
            
            var sorted = productFilter
                .Sort(DbContext.Products)
                .ToArray();
            
            Assert.All(sorted, 
                x => Assert.Equal(sorted.IndexOf(x), sortedExpected.IndexOf(x)));
        }
        
        [Fact]
        public void Filter()
        {
            var productFilter = new PagedProductFilter()
            {
                Search = DbContextFixture.FirstProductName,
                Asc = false
            };

            var results = productFilter
                .Filter(DbContext.Products)
                .ToArray();
            
            Assert.Collection(results, 
                x => Assert.Equal(DbContextFixture.FirstProductName, x.Name));
        }


    }
}