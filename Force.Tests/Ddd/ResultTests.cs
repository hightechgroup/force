using System;
using Force.Ddd;
using Force.Reflection;
using Xunit;

namespace Force.Tests.Ddd
{
    public class ResultTests
    {
        [Fact]
        public void Select()
        {
            var r = new Result<int, string>("false");
            r = r.Select(x => x + 5);
            
            Assert.True(r.IsFaulted);
        }

        [Fact]
        public void SelectMany()
        {
            var r = new Result<int, string>(1);
            r = r.SelectMany(
            x => new Result<int, string>(5), 
            (i, i1) => i + i1);
            
            var res = r.Match(x => x, x => 0);
            Assert.Equal(6, res);
        }
        
        [Fact]
        public void Linq_SuccessAndFailure_ReturnsFailure()
        {
            var r1 = new Result<int, string>(1);
            var r2 = new Result<int, string>("false");

            var a = from r11 in r1 select r1;
            var b = 
                from r11 in r1 
                from r22 in r2     
                select r11 + r22;

            Assert.True(b.IsFaulted);
        }

        [Fact]
        public void New()
        {
            var r1 = new Result<int>(1);
            var r2 = new Result<int>("failure");
            var r3 = new Result<int>(new Exception("exception"));
            
            Assert.False(r1.IsFaulted);
            Assert.True(r2.IsFaulted);
            Assert.True(r3.IsFaulted);
            
            Assert.Equal("failure", r2.Match(x => "success", x => x));
            Assert.Equal("exception", r3.Match(x => "success", x => x));
        }
    }
}