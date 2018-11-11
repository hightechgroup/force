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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings(w =>
            {
                w.Throw(RelationalEventId.QueryClientEvaluationWarning);
            });
        }
    }
}