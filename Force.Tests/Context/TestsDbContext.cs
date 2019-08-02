using Force.Tests.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Force.Tests.Context
{
    public class TestsDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public TestsDbContext(DbContextOptions options) : base(options)
        {            
        }

    }
}