using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Force.Ddd;
using Force.Ddd.Pagination;

namespace Demo.WebApp.Features.Products
{
    public class ProductListQuery
        : IFilter<ProductListDto>
        , IOrder<ProductListDto>
        , IPaging
        , IValidatableObject
    {
        private Spec<ProductListDto> _spec;
        
        public int Page { get; set; }
        
        public int Take { get; set; }
        
        public Spec<ProductListDto> Spec => _spec ?? (_spec = new Spec<ProductListDto>(x => true));
        
        public Sorter<ProductListDto> Sorter { get; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new System.NotImplementedException();
        }
    }
}