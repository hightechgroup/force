using System;
using CostEffectiveCode.Ddd;
using CostEffectiveCode.Ddd.Pagination;
using CostEffectiveCode.Ddd.Specifications;
using Xunit;

namespace CostEffectiveCode.Tests
{
    public class PagingTests
    {
        readonly IdPaging<Product> _paging = new IdPaging<Product>();

        [Fact]
        public void DefaultState_IsValid()
        {
            var paging = new IdPaging<Product>();
            Assert.True(paging.Page == 1);
            Assert.True(paging.Take > 0);
            Assert.True(paging.OrderBy != null);
        }

        [Fact]
        public void SetStateInConstructor_IsValid()
        {
            var paging = new IdPaging<Product>(3, 50);
            Assert.True(paging.Page == 3);
            Assert.True(paging.Take == 50);
            Assert.True(paging.OrderBy != null);
        }

        [Fact]
        public void SetBadValues_Exception()
        {
            Assert.Throws<ArgumentException>(() => _paging.Page = -100);
            Assert.Throws<ArgumentException>(() => _paging.Page = 0);
            Assert.Throws<ArgumentException>(() => _paging.Take = -100);
            Assert.Throws<ArgumentException>(() => _paging.Take = 0);
        }

        [Fact]
        public void SetGoodValues_Success()
        {
            _paging.Page = 10;
            _paging.Take = 23;
        }
    }
}
