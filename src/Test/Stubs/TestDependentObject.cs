using System.Collections.Generic;
using System.Linq;
using CostEffectiveCode.Extensions;
using CostEffectiveCode.Ddd.Specifications;
using CostEffectiveCode.Cqrs;
using CostEffectiveCode.Ddd.Pagination;

namespace CostEffectiveCode.Tests
{
    public class TestDependentObject
    {
        public TestDependentObject(IQuery<IdPaging<ProductDto>, IPagedEnumerable<ProductDto>> pagedQuery
            , IQuery<object, IEnumerable<ProductDto>> projectionQuery
            , IQuery<int, ProductDto> getQuery
            , IHandler<ProductDto, int> createOrUpdateCommandHandler)
        {
        }
    }
}
