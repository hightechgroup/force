using System;
using System.Linq.Expressions;
using CostEffectiveCode.Ddd;
using CostEffectiveCode.Ddd.Pagination;
using Xunit;

namespace CostEffectiveCode.Tests
{
    public class SortingTests
    {
        [Fact]
        public void Sorting()
        {
            Expression<Func<Product, int>> expr = x => x.Id;
            var sort = new Sorting<Product, int>(expr, SortOrder.Desc);
            Assert.Equal(expr, sort.Expression);
            Assert.Equal(SortOrder.Desc, sort.SortOrder);
        }
    }
}
