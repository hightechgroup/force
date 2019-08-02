using System.Diagnostics;
using Force.Ddd;
using Xunit;

namespace Force.Tests
{
    public class SpecTests
    {
        [Fact]
        public void A()
        {
            var spec = new Spec<string>(x => x.ToString().Length < 5);

            var sw = new Stopwatch();
            sw.Start();
            
            spec.IsSatisfiedBy("string");
            var e1 = sw.Elapsed;
            sw.Restart();
            
            var e2 = sw.Elapsed;
            spec.IsSatisfiedBy("string");
            
            Assert.True(e2 * 10 < e1);
        }
    }
}