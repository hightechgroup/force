using System.Collections.Generic;
using System.Linq;
using Force.Linq;
using Force.Tests.Context;
using Force.Tests.Infrastructure;
using Xunit;

namespace Force.Tests.Expressions
{
    public class AutoFilterTests : IClassFixture<DbContextFixture>
    {
        private readonly DbContextFixture _dbContextFixture;

        public AutoFilterTests(DbContextFixture dbContextFixture)
        {
            _dbContextFixture = dbContextFixture;
        }

        [Fact]
        public void Order()
        {
            var filter = new AutoFilter<Product>(new ProductFilter(), "Name DESC");

            var res = filter
                .Order(_dbContextFixture.DbContext.Products)
                .ToList();

            bool flag = true;
            string current = null;
            foreach (var p in res)
            {
                if (current == null)
                {
                    current = p.Name;
                    continue;
                }

                flag = string.Compare(p.Name, current) <= 0;
                if (!flag)
                {
                    break;
                }
            }
            
            Assert.True(flag);
        }
        
        [Theory]
        [MemberData(nameof(Data))]
        public void Filter(TestCase<ProductFilter, List<Product>> testCase)
        {
            var filter = new AutoFilter<Product>(testCase.Input);
            
            var res = filter
                .Filter(_dbContextFixture.DbContext.Products)
                .ToList();

            testCase.Assert(res);
        }

        public static IEnumerable<object[]> Data => TestCaseBuilder
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