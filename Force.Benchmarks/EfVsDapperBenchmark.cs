using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Account = Force.Benchmarks.Data.Account;
using DemoAppDbContext = Force.Benchmarks.Data.DemoAppDbContext;

namespace Force.Benchmarks
{
    public class EfVsDapperBenchmark
    {
        private static string ConnectionString =
            "Server=.;Initial Catalog=Force;Persist Security Info=False;Integrated Security=True;MultipleActiveResultSets=False;Connection Timeout=30;";
        
        private DbContextOptionsBuilder<DemoAppDbContext> _optionsBuilder;

        [Benchmark]
        public void Dapper()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Query<Account>("SELECT * FROM [Accounts]");
            }
        }

        private static readonly Func<DemoAppDbContext, IEnumerable<Account>> CompiledQuery =
            EF.CompileQuery((DemoAppDbContext c) => c.Set<Account>().AsNoTracking());

        [Benchmark]
        public void Ef()
        {
            using (var dbContext = new DemoAppDbContext(_optionsBuilder.Options))
            {
                dbContext
                    .Set<Account>()
                    .AsNoTracking()
                    .ToList();
            }
        }
        
        [Benchmark]
        public void Compiled()
        {
            using (var dbContext = new DemoAppDbContext(_optionsBuilder.Options))
            {
                CompiledQuery(dbContext);
            }
        }

        
        [GlobalSetup]
        public void Setup()
        {
            _optionsBuilder = new DbContextOptionsBuilder<DemoAppDbContext>();
            _optionsBuilder.UseSqlServer(ConnectionString);
        }
        

    }
}