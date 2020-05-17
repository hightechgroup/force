using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Force.Ddd;
using Force.Extensions;
using Force.Tests.Infrastructure.Context;
using Xunit;

namespace Force.Tests.Extensions
{
    public class FunctionalExtensionsTests
    {
        [Fact]
        public async Task AwaitAndPipeTo()
        {
            var res = await Task.FromResult(1).AwaitAndPipeTo(x => x + 1);
            Assert.Equal(2, res);
        }

        [Fact]
        public void EnsureInvariant()
        {
            Assert.Throws<ValidationException>(() =>
            {
                var pr = new Product(null, null);
            });
        }
        
        [Fact]
        public void EitherOr_Func()
        {
            Product product = null;
            Assert.Equal("true", true.EitherOr(x => "true", x => "false"));
            Assert.Equal("false", product.EitherOr(x => "true", x => "false"));
            Assert.Equal("true", product.EitherOr(true, x => "true", x => "false"));
            Assert.Equal("false", product.EitherOr(false, x => "true", x => "false"));
        }
    }
}