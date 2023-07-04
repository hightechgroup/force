using System;
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
        public QueryableTests(DbContextFixture dbContextFixture)
            :base(dbContextFixture)
        {
        }

        [Fact]
        public void OrderByAndOrderByDescending_PropertyDoesntExist_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                DbContext
                    .Products
                    .OrderBy("Weird Property")
                    .ToList();
            });
            
            Assert.Throws<ArgumentException>(() =>
            {
                DbContext
                    .Products
                    .OrderByDescending("Weird Property")
                    .ToList();
            });
        }

        [Fact]
        public void OrderById()
        {
            DbContext
                .Products
                .OrderById();
        }

        [Fact]
        public void FirstOrDefaultById()
        {
            DbContext
                .Products
                .FirstOrDefaultById(1, x => new ProductListItem()
                {
                    Id = x.Id
                });
        }
        
        public static IEnumerable<object[]> OrderData => TestCaseBuilder
            .For<string, List<Product>>()
            .Add("Name", x => OrderChecker.CheckOrder(x, true))
            .Add("Name asc", x => OrderChecker.CheckOrder(x, true))
            .Add("Name.asc", x => OrderChecker.CheckOrder(x, true))
            .Add("naMe AsC", x => OrderChecker.CheckOrder(x, true))
            .Add("Name desc", x => OrderChecker.CheckOrder(x, false))
            .Add("naMe.DesC", x => OrderChecker.CheckOrder(x, false));

        
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
    }
}