using Force.Tests.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Force.Tests.Context
{
    public class DbContextFixture
    {
        public TestsDbContext DbContext { get; }

        static DbContextFixture()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestsDbContext>();
            optionsBuilder.UseInMemoryDatabase("Force");
            var dbContext = new TestsDbContext(optionsBuilder.Options);
            dbContext.Products.Add(new Product()
            {
                Id = 1,
                Name = "1"
            });
            
            dbContext.Products.Add(new Product()
            {
                Id = 2,
                Name = "123"
            });
            
            dbContext.Products.Add(new Product()
            {
                Id = 3,
                Name = "Abc"
            });

            dbContext.SaveChanges();
        }
        
        public DbContextFixture()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestsDbContext>();
            optionsBuilder.UseInMemoryDatabase("Force");
            DbContext = new TestsDbContext(optionsBuilder.Options);
        }
    }
}