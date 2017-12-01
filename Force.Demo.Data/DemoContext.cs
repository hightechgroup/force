using Force.Demo.Domain;
using Microsoft.EntityFrameworkCore;

namespace Force.Demo.Data
{
    public class DemoContext: DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }
    }
}