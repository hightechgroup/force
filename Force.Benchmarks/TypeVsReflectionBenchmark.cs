using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Force.Infrastructure;

namespace Force.Benchmarks
{
    //[SimpleJob(RunStrategy.Monitoring, launchCount: 10, warmupCount: 3, targetCount: 100)]
    public class TypeVsReflectionBenchmark
    {
        [Benchmark]
        public void Type()
        {
            var methods = Type<string>.PublicMethods;
        }

        [Benchmark]
        public void Reflection()
        {
            typeof(string).GetMethods()
                .Where(x => x.IsPublic && !x.IsAbstract)
                .ToArray();
        }
    }
}