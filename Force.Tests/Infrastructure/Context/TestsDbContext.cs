using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Force.Tests.Infrastructure.Context
{
    public class TestsDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var lf = new LoggerFactory();
            lf.AddProvider(new MyLoggerProvider());
            //optionsBuilder.UseLoggerFactory(lf);
        }
        
        public TestsDbContext(DbContextOptions options) : base(options)
        {            
        }

    }
}