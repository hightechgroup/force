using Force.Ddd;
using Force.Tests.Expressions;
using Force.Tests.Infrastructure.Context;
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