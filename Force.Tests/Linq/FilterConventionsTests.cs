using Force.Linq;
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
            FilterConventions.Initialize();
        }
    }
}