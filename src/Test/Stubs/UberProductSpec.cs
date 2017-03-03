using System.Linq;
using CostEffectiveCode.Cqrs.Queries;
using CostEffectiveCode.Ddd.Specifications;
using CostEffectiveCode.Cqrs;
using CostEffectiveCode.Ddd.Pagination;

namespace CostEffectiveCode.Tests
{
    public class UberProductSpec
        : IdPaging<ProductDto>
        , ILinqSpecification<Product>
        , ILinqSpecification<ProductDto>
    {
        public decimal Price = 5;

        public UberProductSpec(int page, int take) : base(page, take)
        {
        }

        public UberProductSpec()
        {
        }

        public IQueryable<Product> Apply(IQueryable<Product> query)
            => query.Where(x => x.Price > Price);

        public IQueryable<ProductDto> Apply(IQueryable<ProductDto> query)
            => query.Where(x => x.CategoryName.Length >= 1);

    }
}
