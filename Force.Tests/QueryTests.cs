﻿using System.Linq;
using Force.Ddd.Pagination;
using Force.Extensions;
using Xunit;

namespace Force.Tests
{
    public class QueryTests: DbContextTestsBase
    {
        [Fact]
        public void PagedQuery_FilterAndSort()
        {
//            var products = DbContext.Products.FilterAndSort(new PagedQuery<Product>());            
//            Assert.True(products.First().Id == 1);
        }
        
        [Fact]
        public void PagedQuery_FilterSortAndPaginate()
        {
//            var products = DbContext.Products.FilterSortAndPaginate(new PagedQuery<Product>());
//            Assert.Equal(2, products.Total);
        }        
    }
}