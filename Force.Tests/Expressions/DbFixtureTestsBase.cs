using Force.Tests.Context;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Force.Tests.Expressions
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