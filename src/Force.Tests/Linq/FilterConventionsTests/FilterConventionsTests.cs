using System;
using System.Threading;
using System.Threading.Tasks;
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

        [Fact]
        public void ParallelInitialization_OnlyFirst()
        {
            var instance1 = new FilterConventionsTest();
            var result1 = FilterConventionsTest.SetInstanceTest(instance1);

            var instance2 = new FilterConventionsTest();
            var result2 = FilterConventionsTest.SetInstanceTest(instance2);
            
            //first instance
            Assert.Equal(instance1, result1);
            //only first instance
            Assert.Equal(result1, result2);
        }
    }

    internal class FilterConventionsTest : FilterConventions
    {
        public static FilterConventions SetInstanceTest(FilterConventionsTest instance) => SetInstance(instance);
    }
}