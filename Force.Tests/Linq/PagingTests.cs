using System;
using System.Linq;
using Force.Linq.Pagination;
using Force.Tests.Expressions;
using Force.Tests.Infrastructure.Context;
using Xunit;

namespace Force.Tests.Linq
{
    public class PagingTests: DbContextFixtureTestsBase
    {
        public PagingTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
        {
        }

        [Fact]
        public void PageIsZero_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var p = new Paging(1, 1)
                {
                    Page = 0,
                    Take = 1
                };           
            });
        }

        [Fact]
        public void TakeIsZero_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var p = new Paging(1, 1)
                {
                    Page = 1,
                    Take = 0
                };
            });
        }

        [Fact]
        public void Total()
        {
            var paging = new Paging();
            paging = new Paging(1, 1)
            {
                Page = 1,
                Take = 1
            };
            var res = DbContext
                .Products
                .OrderBy(x => x.Id)
                .ToPagedEnumerable(paging);
            
            Assert.Single(res.Items);
            Assert.Equal(1, res.Total);
        }
    }
}