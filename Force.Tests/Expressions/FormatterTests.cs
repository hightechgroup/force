using System.Linq;
using Force.Ddd;
using Force.Tests.Context;
using Xunit;

namespace Force.Tests.Expressions
{
    public class FormatterTests: DbFixtureTestsBase
    {
        public FormatterTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
        {
        }
        
        [Fact]
        public void A()
        {
            var formatter = new Formatter<Product>(x => "Product Name is: " + x.Name);
            var res = DbContext
                .Products
                .Select<Product, string>(formatter)
                .ToList();

            Assert.True(res.All(x => x.Any() && x.StartsWith("Product Name is: ")));
        }

    }
}