using Force.Linq;
using Xunit;

namespace Force.Tests.Linq
{
    public class SorterTests
    {
        [Fact]
        public void TryParse()
        {
            Sorter<string>.TryParse("Length", out var s);
            Sorter<string>.TryParse("W", out var s2);
        }
    }
}