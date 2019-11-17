using Force.Tests.Infrastructure.Context;
using Xunit;

namespace Force.Tests.Cqrs
{
    public class FilterQueryTests
    {
        [Fact]
        public void A()
        {
            var productFilter = new ProductFilter()
            {
                Search = "123",
                Order = "132",
                Asc = false
            };
        }
        
        [Fact]
        public void B()
        {
            var productFilter = new PagedProductFilter()
            {
                Page = 1,
                Take = 1,
                Search = "123",
                Order = "132",
                Asc = false
            };

            var asc = productFilter.Asc;
            var s = productFilter.Search;
            var o = productFilter.Order;

            var p = productFilter.Page;
            var t = productFilter.Take;
        }

        [Fact]
        public void Sort()
        {
            var productFilter = new PagedProductFilter()
            {
                Search = "123",
                Order = "132",
                Asc = false
            };
            
            
            productFilter.Sort(null);
        }
        
        [Fact]
        public void Filter()
        {
            var productFilter = new PagedProductFilter()
            {
                Search = "123",
                Order = "132",
                Asc = false
            };
            
            
            productFilter.Filter(null);
        }
    }
}