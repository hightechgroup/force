using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Force.Extensions;

namespace Force.Benchmarks
{
    [SimpleJob(RunStrategy.Monitoring, launchCount: 10, warmupCount: 3, targetCount: 100)]
    public class InvariantBenchmarks
    {
        [Benchmark]
        public void EnsureInvariantAllTrue()
        {
            var username = new UserName("1", "2", "3");
            username.EnsureInvariant();
        }
        
        [Benchmark]
        public void EnsureInvariantAllFalse()
        {
            var username = new UserName("1", "2", "3");
            username.EnsureInvariant(false);
        }
        
        [Benchmark]
        public void EnsureInvariantException()
        {
            var username = new UserName(null, null, null);
            try
            {
                username.EnsureInvariant();
            }
            catch
            {
            }
        }
        
        [Benchmark]
        public void NoInvariant()
        {
            var username = new UserName("1", "2", "3");
        }
        
        [Benchmark]
        public void ManualInvariant()
        {
            var username = new UserName("1", "2", "3");
            if (string.IsNullOrEmpty(username.FirstName))
            {
                throw new ArgumentException();
            }
            
            if (string.IsNullOrEmpty(username.LastName))
            {
                throw new ArgumentException();
            }
            
            if (string.IsNullOrEmpty(username.MiddleName))
            {
                throw new ArgumentException();
            }
        }
    }
}