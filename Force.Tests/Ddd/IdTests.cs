using Force.Ddd;
using Force.Tests.Expressions;
using Xunit;

namespace Force.Tests.Ddd
{
    public class IdTests
    {
        [Fact]
        public void TryParse_()
        {
            Id<Product>.TryParse(1, x => null, out var id);
            int intid = id;
        }
    }
}