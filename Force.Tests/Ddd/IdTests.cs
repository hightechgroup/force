using System;
using System.Linq;
using Force.Ddd;
using Force.Linq;
using Force.Tests.Infrastructure.Context;
using Xunit;

namespace Force.Tests.Ddd
{
    public class IdTests: DbContextFixtureTestsBase
    {
        public IdTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
        {
        }
        
        [Fact]
        public void New_LoaderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var id = new Id<Product>(1, null);
            });
            
            Assert.Throws<ArgumentNullException>(() =>
            {
                var id = new Id<int, Product>(1, null);
            });
        }

        [Fact]
        public void New_ValueIsZero_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var id = new Id<Product>(0, x => null);
            });
        }

        [Fact]
        public void New_ExistingEntity_CreatesNewInstance()
        {
            var id = new Id<Product>(DbContext.Products.First());
        }
        
        [Fact]
        public void New_CorrectValueAndLoader_CreatesNewInstance()
        {
            var id = new Id<Product>(1, x => null);
        }
        
        [Fact]
        public void TryParse_ValueIsZeroReturnsFalseIdIsNull()
        {
            var res = Id<Product>.TryParse(0, x => null, out var id);
            
            Assert.False(res);
            Assert.Null(id);
        }
        
       
        [Fact]
        public void TryParse_ValueIsValid_EntityIsNotNull()
        {
            Id<Product>.TryParse(1, x => DbContext.Products.FirstOrDefaultById(x), out var id);
            Assert.NotNull(id.Entity);
        }

        [Fact]
        public void Implicit_KeyValue()
        {
            Id<Product>.TryParse(1, x => null, out var id);
            int intId = id;
            Assert.Equal(1, intId);
            
            Id<int, Product>.TryParse(1, x => null, out var id2);
            intId = id2;
            Assert.Equal(1, intId);
        }

        [Fact]
        public void Implicit_Entity()
        {
            var id = new Id<Product>(DbContext.Products.First());
            Product entity = id;
            id = entity;
            
            Assert.Equal(entity, id.Entity);
        }
    }
}