using System;
using Force.Linq;
using Force.Linq.Conventions;
using Xunit;

namespace Force.Tests.Linq
{
    public class FilterConventionsTests
    {
        [Fact]
        public void Instance()
        {
            var i = FilterConventions.Instance;
            var i2 = FilterConventions.Instance;
           
            Assert.Equal(i, i2);
        }

        [Fact]
        public void DoubleInitialization_ThrowsInvalidArgumentException()
        {
            var i = FilterConventions.Instance;
            Assert.Throws<InvalidOperationException>(() =>
            {
                FilterConventions.Initialize();
            });
        }
    }
}