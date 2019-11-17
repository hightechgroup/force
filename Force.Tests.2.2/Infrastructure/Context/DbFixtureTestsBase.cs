using Xunit;

namespace Force.Tests.Infrastructure.Context
{
    public class DbFixtureTestsBase : IClassFixture<DbContextFixture>
    {
        protected DbContextFixture DbContextFixture;

        public TestsDbContext DbContext => DbContextFixture.DbContext;

        public DbFixtureTestsBase(DbContextFixture dbContextFixture)
        {
            DbContextFixture = dbContextFixture;
        }
    }
}