using System.Linq;
using Force.Linq;
using Force.Tests.Infrastructure.Context;
using Xunit;

namespace Force.Tests.Expressions
{
    public class SorterTests: DbContextFixtureTestsBase
    {
        public SorterTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
        {
        }
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Name_AscDesc(bool asc)
        {
            var sorter = new Sorter<Product, string>(x => x.Name, asc);
            var res = DbContext
                .Products
                .Sort(sorter)
                .ToList();

            OrderChecker.CheckOrder(res, asc);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Name_ToExpressions_AscDesc(bool asc)
        {
            var sorter = new Sorter<Product, string>(x => x.Name, asc);
            var res = DbContext
                .Products
                .OrderBy<Product, string>(sorter)
                .ToList();

            OrderChecker.CheckOrder(res, asc);
        }

    }
}