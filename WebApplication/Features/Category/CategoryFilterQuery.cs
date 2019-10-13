using System.Linq;
using Force.Cqrs;
using Force.Ddd;
using Force.Linq;

namespace WebApplication.Features.Category
{
    public class CategoryFilterQuery: PagedFilterQuery<ProductListItem>
    {
        public int CategoryId { get; set; }
        
        public int BrandId { get; set; }
        
        public decimal MinPrice { get; set; }
        
        public decimal MaxPrice { get; set; }

        public override IQueryable<ProductListItem> Filter(IQueryable<ProductListItem> queryable)
            => base
                .Filter(queryable)
                .WhereIf(MinPrice > 0, x => x.Price >= MinPrice)
                .WhereIf(MaxPrice < 0, x => x.Price >= MaxPrice);
    }
}