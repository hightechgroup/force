using Microsoft.EntityFrameworkCore;

namespace Force.Tests
{
    public class TestDbContext: DbContext
    {
        public TestDbContext(DbContextOptions options) : base(options)
        {
            
        }        
        
        public DbSet<Product> Products { get; set; }
    }
}