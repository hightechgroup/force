using System.Linq;
using Force.Linq;
using Force.Tests.Infrastructure.Context;
using Xunit;

namespace Force.Tests.Linq
{
    public class SorterTests
    {
        [Fact]
        public void Ctr()
        {
            var s = new Sorter<string>("Wrong");
        }
        
        [Fact]
        public void TryParse()
        {
            Sorter<string>.TryParse("Length", out var s);
            Sorter<string>.TryParse("W", out var s2);
        }

        [Fact]
        public void TryParseW()
        {
            Sorter<string>.TryParse("Length W", out var s2);
        }

        [Fact]
        public void Sort()
        {
//            var p = new[] {new Product() {Name = "123"}};
//            var s = new Sorter<Product>("Name");
//            s.Sort(p.AsQueryable());
        }
    }
}