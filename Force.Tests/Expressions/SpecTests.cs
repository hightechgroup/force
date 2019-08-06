using System;
using System.Linq;
using Force.Ddd;
using Force.Tests.Context;
using Xunit;

namespace Force.Tests.Expressions
{
    public class SpecTests: DbFixtureTestsBase
    {
        private static readonly Spec<Product> spec1 = new Spec<Product>(x => x.Id > 0);
        private static readonly Spec<Product> spec2 = new Spec<Product>(x => x.Name.StartsWith("1"));
 
        public SpecTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
        {
        }
        
        private void TestAnd(Func<Spec<Product>,Spec<Product>,Spec<Product>> andFunc)
        {
            var and = andFunc(spec1, spec2);
            var res = DbContext
                .Products
                .Where(and)
                .ToList();

            Assert.NotEmpty(res);
            Assert.True(res.All(x => x.Id > 0 && x.Name.StartsWith("1")));
        }

        [Fact]
        public void Not()
        {
            var s3 = !spec1;
        }

        [Fact]
        public void And()
        {
            TestAnd((s1, s2) => s1 & s2);
        }
        
        [Fact]
        public void DoubleAnd()
        {
            TestAnd((s1, s2) => s1 && s2);
        }

        [Fact]
        public void Satisfy()
        {
            var res = DbContext
                .Products
                .ToList();

            var list =res
                .Where(x => spec1.IsSatisfiedBy(x))
                .ToList();

            Assert.True(res.All(x => x.Id > 0));
        }
    }
}