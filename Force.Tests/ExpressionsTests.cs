using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Force.Expressions;
using Force.Extensions;
using Force.Tests.Expressions;
using Xunit;

namespace Force.Tests
{
    public class ExpressionsTests
    {
        public  const int CompileMeanPlusStdDev = 80000;
        public  const int AsFuncMeanPlusStdDev = 25;

        [Fact]
        public void AsFunc_Caches()
        {
            var summary = BenchmarkRunner.Run<ExpressionCompilerBenchmark>();

            AssertMean(summary, nameof(ExpressionCompilerBenchmark.Compile), CompileMeanPlusStdDev);
            AssertMean(summary, nameof(ExpressionCompilerBenchmark.AsFunc), AsFuncMeanPlusStdDev);
        }

        private static void AssertMean(Summary summary, string benchmarkName, int mean)
        {
            var report = GetReportByName(summary, benchmarkName);
            Assert.True(report.ResultStatistics.Mean < mean, 
                $"Mean: {report.ResultStatistics.Mean}");
        }

        private static BenchmarkReport GetReportByName(Summary summary, string benchmarkName) =>
            summary
                .Reports
                .First(x => x.BenchmarkCase.Descriptor.DisplayInfo ==
                            nameof(ExpressionCompilerBenchmark) + "." + benchmarkName);

        private static double ElapsedNanoSeconds(Stopwatch sw)
        {
            return ((double)sw.ElapsedTicks / Stopwatch.Frequency) * 1000000000;
        }
    }
}