using BenchmarkDotNet.Running;

namespace Force.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<InvariantBenchmarks>();
        }
    }
}