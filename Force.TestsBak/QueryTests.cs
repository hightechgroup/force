using System.Linq;
using Xunit;

namespace Force.Tests
{
    public class QueryTests: DbContextTestsBase
    {
        [Fact]
        public void A()
        {
            var products = DbContext.Products.ToList();
        }
    }
}