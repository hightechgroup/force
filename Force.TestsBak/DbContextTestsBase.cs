using Microsoft.EntityFrameworkCore;

namespace Force.Tests
{
    public abstract class DbContextTestsBase
    {
        protected readonly TestDbContext DbContext;
            
        protected DbContextTestsBase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
            optionsBuilder.UseInMemoryDatabase("Force");
            
            DbContext = new TestDbContext(optionsBuilder.Options);        
        }
    }
}