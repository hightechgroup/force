using DemoApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DemoApp.Data
{
    public class DemoContext
        : DbContext
        , IDesignTimeDbContextFactory<DemoContext>
    {
        public static readonly string ConnectionString = "Data Source=.;Initial Catalog=force;Integrated Security=True";

        public DemoContext()
        {
        }
        
        public DemoContext(DbContextOptions<DemoContext> options) : base(options)
        {

        }
        
        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }
        
        public DemoContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DemoContext>();
            optionsBuilder.UseSqlServer(ConnectionString);

            return new DemoContext(optionsBuilder.Options);
        }
    }
}