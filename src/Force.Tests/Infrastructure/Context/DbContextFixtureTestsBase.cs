using Xunit;

namespace Force.Tests.Infrastructure.Context
{
    public class DbContextFixtureTestsBase : IClassFixture<DbContextFixture>
    {
        protected DbContextFixture DbContextFixture;

        public TestsDbContext DbContext => DbContextFixture.DbContext;

        public DbContextFixtureTestsBase(DbContextFixture dbContextFixture)
        {
            DbContextFixture = dbContextFixture;
        }
    }
}