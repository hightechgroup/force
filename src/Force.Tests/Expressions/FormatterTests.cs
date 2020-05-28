using System;
using System.Linq;
using System.Linq.Expressions;
using Force.Ddd;
using Force.Tests.Infrastructure.Context;
using Xunit;

namespace Force.Tests.Expressions
{
    public class FormatterTests: DbContextFixtureTestsBase
    {
        public FormatterTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
        {
        }
        
        [Fact]
        public void Select_Format()
        {
            var formatter = new Formatter<Product>(x => "Product Name is: " + x.Name);
            var res = DbContext
                .Products
                .Select<Product, string>(formatter)
                .ToList();
            
            var cf = new Formatter<Category>(x => x.Id.ToString());
            Expression<Func<Product, string>> e = cf.From((Product p) => p.Category);

            Assert.True(res.All(x => x.Any() && x.StartsWith("Product Name is: ")));
        }

    }
}