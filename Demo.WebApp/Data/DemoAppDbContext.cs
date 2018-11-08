using System;
using Demo.WebApp.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Demo.WebApp.Data
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            var converter = new ValueConverter<Email, string>(
                v => v,
                v => v);

            modelBuilder
                .Ignore<Email>();

            modelBuilder
                .Entity<Account>()
                .Property(e => e.Email)
                .HasConversion(converter);
        }
    }
}