using Force.Ddd;
using Force.Tests.Expressions;
using Force.Tests.Infrastructure.Context;
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
            Assert.Null(id.Entity);
            
            Id<int, Product>.TryParse(1, x => null, out var id2);
            int intid2 = id2;
            Assert.Null(id.Entity);

            Product p = id2;
            
            var id3 = new Id<int, Product>(new Product() {Id = 1});
            int intid3 = id3;
            
            var id4 = new Id<Product>(new Product() {Id = 1});
        }
    }
}