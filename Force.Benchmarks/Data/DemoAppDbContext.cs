using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Force.Benchmarks.Data
{
    public class DemoAppDbContext: DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public DemoAppDbContext(DbContextOptions options) : base(options)
        {            
        }
    }
}