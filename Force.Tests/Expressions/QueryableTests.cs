using System.Collections.Generic;
using System.Linq;
using Force.Linq;
using Force.Tests.Infrastructure;
using Force.Tests.Infrastructure.Context;
using Xunit;

namespace Force.Tests.Expressions
{
    public class QueryableTests : DbContextFixtureTestsBase
    {
        static QueryableTests()
        {
            FilterConventions.Initialize(x => { });
        }
        
        public QueryableTests(DbContextFixture dbContextFixture)
            :base(dbContextFixture)
        {
        }

        [Fact]
        public void A()
        {
            DbContext
                .Products
                .WhereIf(true, x => true)
                .FilterAndSort(new ProductFilter())
                .OrderById()
                .FirstOrDefaultById(1, x => new ProductListItem()
                {
                    Id = x.Id
                });
        }
        
        [Theory]
        [MemberData(nameof(OrderData))]
        public void Order(TestCase<string, List<Product>> testCase)
        {
            Sorter<Product>.TryParse(testCase.Input, out var sorter);
            
            var res = DbContext
                .Products
                .Sort(sorter) 
                .ToList();
            
            testCase.Assert(res);
        }

        [Fact]
        public void FilterByConventions_ThrowsArgument()
        {
            DbContextFixture
                .DbContext
                .Products
                .FilterByConventions(null);
        }

        [Theory]
        [MemberData(nameof(FilterData))]
        public void FilterByConventions(TestCase<ProductFilter, List<Product>> testCase)
        {
            var res = 
                DbContextFixture
                .DbContext
                .Products
                .FilterByConventions(testCase.Input)
                .ToList();

            testCase.Assert(res);
        }

        public static IEnumerable<object[]> OrderData => TestCaseBuilder
            .For<string, List<Product>>()
            .Add("Name", x => OrderChecker.CheckOrder(x, true))
            .Add("Name asc", x => OrderChecker.CheckOrder(x, true))
            .Add("Name.asc", x => OrderChecker.CheckOrder(x, true))
            .Add("naMe AsC", x => OrderChecker.CheckOrder(x, true))
            .Add("Name desc", x => OrderChecker.CheckOrder(x, false))
            .Add("naMe.DesC", x => OrderChecker.CheckOrder(x, false));

        public static IEnumerable<object[]> FilterData => TestCaseBuilder
            .For<ProductFilter, List<Product>>()
            .Add(new ProductFilter(), x => x.Any())
            .Add(new ProductFilter()
            {
                Id = 1,
                Name = "1"
            }, x => x.Any() && x.All(y => y.Name.StartsWith("1")))
            .Add(new ProductFilter()
            {
                Id = 1,
            }, x => x.Any() && x.All(y => y.Id == 1))
            .Add(new ProductFilter()
            {
                Id = 2,
                Name = "2"
            }, x => !x.Any())
            .Add(new ProductFilter()
            {
                Id = 2,
            }, x => x.Any())
            .Add(new ProductFilter()
            {
                Name = "a"
            }, x => x.Any() && x.All(y => y.Name.ToLower().StartsWith("a")))        
        ;
    }
}