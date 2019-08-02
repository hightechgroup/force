using Force.Tests.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Force.Tests.Context
{
    public class DbContextFixture
    {
        public TestsDbContext DbContext { get; }
            
        public DbContextFixture()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestsDbContext>();
            optionsBuilder.UseInMemoryDatabase("Force");
            DbContext = new TestsDbContext(optionsBuilder.Options);
            DbContext.Products.Add(new Product()
            {
                Id = 1,
                Name = "1"
            });
            DbContext.SaveChanges();
        }
    }
}