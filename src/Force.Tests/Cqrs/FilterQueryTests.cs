using System.Linq;
using Force.Linq;
using Force.Reflection;
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
        public void Search()
        {
            var productFilter = new PagedProductFilter()
            {
                Search = DbContextFixture.FirstProductName
            };

            var results = DbContext
                .Products
                .Filter(productFilter)
                .ToList();
            
            Assert.Collection(results, 
                x => x.Name.StartsWith(DbContextFixture.FirstProductName));
        }
        
        [Fact]
        public void Sort_Asc()
        {
            var productFilter = new PagedProductFilter()
            {
                Order = "Name",
                Asc = false
            };

            var sortedExpected = DbContext
                .Products
                .OrderByDescending(x => x.Name)
                .Select(x => x.Name)
                .ToArray();
            
            var sorted = productFilter
                .Sort(DbContext.Products)
                .Select(x => x.Name)
                .ToArray();
            
            Assert.All(sorted, 
                x => Assert.Equal(sorted.IndexOf(x), sortedExpected.IndexOf(x)));
        }
        
        [Fact]
        public void Filter()
        {
            var pp = Type<PagedProductFilter>.PublicProperties;
            
            var productFilter = new PagedProductFilter()
            {
                Name = DbContextFixture.FirstProductName
            };

            var results = productFilter
                .Filter(DbContext.Products)
                .ToList();
            
            Assert.Collection(results, 
                x => Assert.Equal(DbContextFixture.FirstProductName, x.Name));
        }
        
        [Fact]
        public void SearchItem()
        {
            var query = new PagedProductFilter()
            {
                Search = DbContextFixture.FirstProductName
            };

            var results = query
                .SearchItem(DbContext.Products)
                .ToList();
            
            Assert.Collection(results, 
                x => Assert.Equal(DbContextFixture.FirstProductName, x.Name));
        }

        [Fact]
        public void SearchBy()
        {
            var query = new PagedProductFilter()
            {
                Search = DbContextFixture.FirstProductName.ToLower(),
                SearchBy = "NaMe"
            };

            var results = query
                .SearchItem(DbContext.Products)
                .ToList();

            Assert.Collection(results,
                x => Assert.Equal(DbContextFixture.FirstProductName, x.Name));
        }
    }
}