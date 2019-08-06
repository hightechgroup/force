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
            var e1 = sw.Elapsed;
            sw.Restart();
            
            var func2 = expr.AsFunc();
            var e2 = sw.Elapsed;

            Assert.Equal(func1, func2);
            Assert.True(e2 * 5 < e1);
        }
    }
}