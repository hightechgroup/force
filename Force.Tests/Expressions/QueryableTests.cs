using System.Collections.Generic;
using System.Linq;
using Force.Linq;
using Force.Tests.Context;
using Force.Tests.Infrastructure;
using Xunit;

namespace Force.Tests.Expressions
{
    public class QueryableTests : DbFixtureTestsBase
    {
        static QueryableTests()
        {
            FilterConventions.Initialize(x => { });
        }
        
        public QueryableTests(DbContextFixture dbContextFixture)
            :base(dbContextFixture)
        {
        }

        [Theory]
        [MemberData(nameof(OrderData))]
        public void Order(TestCase<string, List<Product>> testCase)
        {
            Sorter<Product>.TryParse(testCase.Input, out var sorter);
            
            var res =DbContextFixture
                .DbContext
                .Products
                .Sort(sorter) 
                .ToList();
            
            testCase.Assert(res);
        }

        private static bool CheckOrder(List<Product> res, bool asc)
        {
            bool flag = true;
            string current = null;
            foreach (var p in res)
            {
                if (current == null)
                {
                    current = p.Name;
                    continue;
                }

                flag = asc 
                    ? string.Compare(p.Name, current) >= 0
                    : string.Compare(p.Name, current) <= 0;
                
                if (!flag)
                {
                    break;
                }
            }

            return flag;
        }

        [Theory]
        [MemberData(nameof(FilterData))]
        public void FilterByConvention(TestCase<ProductFilter, List<Product>> testCase)
        {
            var res = 
                DbContextFixture
                .DbContext
                .Products
                .FilterByConvention(testCase.Input)
                .ToList();

            testCase.Assert(res);
        }

        public static IEnumerable<object[]> OrderData => TestCaseBuilder
            .For<string, List<Product>>()
            .Add("Name", x => CheckOrder(x, true))
            .Add("Name asc", x => CheckOrder(x, true))
            .Add("Name.asc", x => CheckOrder(x, true))
            .Add("naMe AsC", x => CheckOrder(x, true))
            .Add("Name desc", x => CheckOrder(x, false))
            .Add("naMe.DesC", x => CheckOrder(x, false));

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