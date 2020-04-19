using System.Linq;
using Force.Ddd;
using Force.Tests.Infrastructure.Context;
using Xunit;

namespace Force.Tests.Ddd
{
    public class HasIdTests: IClassFixture<DbContextFixture>
    {
        private readonly DbContextFixture _dbContextFixture;

        public HasIdTests(DbContextFixture dbContextFixture)
        {
            _dbContextFixture = dbContextFixture;
        }
        
        [Fact]
        public void IsNew()
        {
            var pr = _dbContextFixture.DbContext.Products.First();
            Assert.False(pr.IsNew());

            // ReSharper disable once RedundantCast
            int obj = (int)((IHasId) pr).Id;
            Assert.Equal(1, obj);
        }
    }
}