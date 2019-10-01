using System.Linq;
using Force.Linq.Pagination;
using Force.Tests.Expressions;
using Force.Tests.Infrastructure.Context;
using Xunit;

namespace Force.Tests.Linq
{
    public class PagingTests: DbFixtureTestsBase
    {
        public PagingTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
        {
        }
        
        [Fact]
        public void Total()
        {
            var paging = new Paging();
            paging = new Paging(1, 1);
            var res = DbContext
                .Products
                .OrderBy(x => x.Id)
                .ToPagedEnumerable(paging);
            
            Assert.Equal(1, res.Items.Count());
            Assert.Equal(1, res.Total);
        }
    }
}