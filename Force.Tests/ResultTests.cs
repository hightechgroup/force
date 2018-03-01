using System;
using Force.Ddd;
using Xunit;

namespace Force.Tests
{
    public class ResultTests
    {
        [Fact]
        public void And()
        {
            var res = Result.Fail("Fail") && Exception();
            Assert.True(res.IsFaulted);

            var isException = false;
            try
            {
                res = Result.Success && Exception();
            }
            catch (InvalidOperationException)
            {
                isException = true;
            }
            
            Assert.True(isException);
        }

        [Fact]
        public void Or()
        {
            var res = Result.Success || Exception();
            
            Assert.False(res.IsFaulted);

            var isException = false;
            try
            {
                res = Result.Fail("fail") || Exception();
            }
            catch (InvalidOperationException)
            {
                isException = true;
            }
            
            Assert.True(isException);        
        }

        private Result Exception()
        {
            throw new InvalidOperationException();
        }
    }
}