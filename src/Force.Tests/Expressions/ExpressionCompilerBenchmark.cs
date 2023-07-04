using System;
using System.Linq.Expressions;
using BenchmarkDotNet.Attributes;
using Force.Expressions;

namespace Force.Tests.Expressions
{
    //[SimpleJob(RunStrategy.Monitoring, launchCount: 10, warmupCount: 3, targetCount: 100)]
    [MemoryDiagnoser]
    public class ExpressionCompilerBenchmark
    {
        Expression<Func<string, bool>> _expression = x => x.ToString().Length > 5;
            
        [Benchmark]
        public void Compile()
        {
            _expression.Compile();
        }

        [Benchmark]
        public void AsFunc()
        {
            _expression.AsFunc();
        }
    }
}