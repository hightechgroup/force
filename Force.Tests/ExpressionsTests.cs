using System;
using System.Diagnostics;
using System.Linq.Expressions;
using Force.Expressions;
using Force.Extensions;
using Xunit;

namespace Force.Tests
{
    public class ExpressionsTests
    {
        [Fact]
        public void AsFunc_Caches()
        {
            Expression<Func<string, bool>> expr = x => x.ToString().Length > 5;
            var sw = new Stopwatch();
            
            sw.Start();
            var func1 = expr.AsFunc();
            var e1 = ((double)sw.ElapsedTicks / Stopwatch.Frequency) * 1000000000;
            sw.Restart();
            
            var func2 = expr.AsFunc();
            var e2 = ((double)sw.ElapsedTicks / Stopwatch.Frequency) * 1000000000;

            Assert.Equal(func1, func2);
            Assert.True(e2 < 50000 && e2 * 10 < e1, $"Compile: {e1} | Cache: {e2}");
        }
    }
}