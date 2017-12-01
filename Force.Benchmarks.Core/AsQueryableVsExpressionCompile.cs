using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BenchmarkDotNet.Attributes;

namespace Force.Benchmarks.Core
{
    public class AsQueryableVsExpressionCompile
    {
        private static List<CanBeActive> CanBeActiveList = new List<CanBeActive>()
        {
            new CanBeActive(true),
            new CanBeActive(false),
            new CanBeActive(true),
            new CanBeActive(false),
            new CanBeActive(true),
        };

        private static Expression<Func<CanBeActive, bool>> IsActive = x => x.IsActive;

        private static Func<CanBeActive, bool> IsActiveFunc = IsActive.Compile();

        [Benchmark]
        public void Compile()
        {
            IsActive.Compile();
        }
        
        [Benchmark]
        public void AsQueryable()
        {
            CanBeActiveList.AsQueryable();
        }
        
        [Benchmark]
        public void ListWhere()
        {
            CanBeActiveList
                .Where(IsActiveFunc)
                .ToList();
        }
        
        [Benchmark]
        public void AsQueryableWhereToList()
        {
            CanBeActiveList
                .AsQueryable()
                .Where(IsActive)
                .ToList();
        }
    }
}