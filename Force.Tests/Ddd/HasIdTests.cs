using Force.Ddd;
using Force.Tests.Expressions;
using Xunit;

namespace Force.Tests.Ddd
{
    public class HasIdTests
    {
        [Fact]
        public void IsNew()
        {
            var pr = new Product();
            pr.IsNew();
        }
    }
}