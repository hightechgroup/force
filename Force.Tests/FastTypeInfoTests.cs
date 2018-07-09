using Force.Infrastructure;
using Force.Tests;
using Xunit;

namespace Force.AspNetCore.Mvc
{
    public class FastTypeInfoTests
    {
        [Fact]
        public void Properties()
        {
            var props = FastTypeInfo<ProductFilter>.PublicProperties;
            Assert.True(props.Length > 0);
        }
    }
}