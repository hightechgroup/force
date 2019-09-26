using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Force.Ddd;
using Force.Extensions;
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
        public void EitherOr_Func()
        {
            var a = true;
            a.EitherOr(x => !x, x => !x);
            a.EitherOr(x => !x, x => !x, x => !x);
        }
        
        [Fact]
        public void EitherOr_Value()
        {
            var a = true;
            a.EitherOr(a, x => !x, x => x);
            a.EitherOr(x => true, x => !x, x => x);
            a.EitherOr(x => !x, x => x);
        }

        [Fact]
        public void ValidationResultsExtensions()
        {
            IEnumerable<ValidationResult> results = new List<ValidationResult>();
            var r = results.IsValid();
            Assert.NotNull(r);
        }
        
        [Fact]
        public void Merge()
        {
            
        }
    }
}