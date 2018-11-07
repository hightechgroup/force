using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Force.Ddd;
using Force.Ddd.Pagination;

namespace Demo.WebApp.Features.Products
{
    public class ProductListQuery
        : IFilter<ProductListDto>
        , ISorter<ProductListDto>
        , IPaging
        , IValidatableObject
    {
        private Spec<ProductListDto> _spec;
        
        public int Page { get; set; }
        
        public int Take { get; set; }
        
        public Spec<ProductListDto> Spec => _spec ?? (_spec = new Spec<ProductListDto>(x => true));

        private Sorter<ProductListDto> _sorter;
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new System.NotImplementedException();
        }

        public IOrderedQueryable<ProductListDto> Sort(IQueryable<ProductListDto> queryable)
            => _sorter.Sort(queryable);
    }
}