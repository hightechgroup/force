using System;
using System.Collections.Generic;
using System.Linq;
using Force.Ddd;
using Force.Linq;
using Force.Tests.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Force.Tests.Ddd
{
    public class SpecTests: DbContextFixtureTestsBase
    {
        private static readonly Spec<Product> Spec1 
            = new Spec<Product>(x => x.Name == DbContextFixture.FirstProductName);
        
        private static readonly Spec<Product> Spec2 
            = new Spec<Product>(x => x.Id < DbContextFixture.LastProductId);
 
        public SpecTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
        {
        }
        
        private void TestComposed(Func<Spec<Product>, Spec<Product>,Spec<Product>> composeFunc, 
            Action<IList<Product>> assert)
        {
            var composed = composeFunc(Spec1, Spec2);
            var res = DbContext
                .Products
                .Where(composed)
                .ToList();

            Assert.NotEmpty(res);
            assert(res);
        }

        [Fact]
        public void From()
        {
            var s = new Spec<Category>(x => x.Name == DbContextFixture.FirstCategoryName);
            var from = s.From<Product>(x => x.Category);

            var products = DbContext
                .Products
                .Include(x => x.Category)
                .Where(from)
                .ToList();
            
            Assert.All(products, x => 
                Assert.Equal(x.Category.Name, DbContextFixture.FirstCategoryName));
        }
        
        [Fact]
        public void Not()
        {
            var s3 = !Spec1;

            var products = DbContext
                .Products
                .Where(s3)
                .ToList();
            
            Assert.All(products, x => Assert.NotEqual(DbContextFixture.FirstCategoryName, x.Name));
        }

        private void AssertOr(IList<Product> list)
        {
            Assert.All(list, x =>
            {
                Assert.True(x.Name.StartsWith(DbContextFixture.FirstProductName)
                            || x.Name.StartsWith(DbContextFixture.SecondProductName));
            });
        }
        
        [Fact]
        public void Or()
        {
            TestComposed((s1, s2) => s1 | s2, AssertOr);
        }
        
        [Fact]
        public void DoubleOr()
        {
            TestComposed((s1, s2) => s1 || s2, AssertOr);
        }
        
        private void AssertAnd(IList<Product> list)
        {
            Assert.All(list, x =>
            {
                Assert.True(x.Name.StartsWith(DbContextFixture.FirstProductName)
                            || x.Name.StartsWith(DbContextFixture.LastProductName));
            });
        }
        
        [Fact]
        public void And()
        {
            TestComposed((s1, s2) => s1 & s2, AssertAnd);
        }
        
        [Fact]
        public void DoubleAnd()
        {
            TestComposed((s1, s2) => s1 && s2, AssertAnd);
        }

        [Fact]
        public void Satisfy()
        {
            var res = DbContext
                .Products
                .ToList();

            var list =res
                .Where(x => Spec1.IsSatisfiedBy(x))
                .ToList();

            Assert.True(res.All(x => x.Id > 0));
        }

        [Fact]
        public void BuildOr()
        {
            var spec = SpecBuilder<Product>.Build(
                new
                {
                    Id = 1, Name = DbContextFixture.SecondProductName
                }, 
                ComposeKind.Or);

            var res = DbContext
                .Products
                .Where(spec)
                .ToList();
            
            Assert.All(res, x => Assert.True(x.Name == DbContextFixture.FirstProductName
                                             || x.Name == DbContextFixture.SecondProductName));
        }
    }
}