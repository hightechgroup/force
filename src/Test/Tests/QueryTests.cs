using System.Collections.Generic;
using System.Linq;
using CostEffectiveCode.Cqrs;
using CostEffectiveCode.Cqrs.Queries;
using CostEffectiveCode.Ddd;
using CostEffectiveCode.Ddd.Specifications;
using CostEffectiveCode.AutoMapper;
using CostEffectiveCode.Tests.Stubs;
using Xunit;

namespace CostEffectiveCode.Tests
{
    public class QueryTests
    {
        static QueryTests()
        {
            StaticAutoMapperWrapper.Init(cfg => cfg.CreateMissingTypeMaps = true);
        }

        private static Category _category = new Category(10, "Cat");

        static Product[] _products = new[]
        {
            new Product() { Id = 5, Category = _category, Name = "123", Price = 100500 }
            , new Product() { Category = _category, Name = "234", Price = 1 }
        };

        static InMemoryLinqProvider _linqProvider = new InMemoryLinqProvider(_products);

        PagedQuery<Product, ProductDto, int> _productQuery
            = new PagedQuery<Product, ProductDto, int>(_linqProvider
                , new StaticAutoMapperWrapper());

        [Fact]
        public void PagedQuery_Ask_TotalCountAndResultAreRight()
        {
            var spec = new UberProductSpec();
            var res = _productQuery.Ask(spec);
            var nonPagedRes = _productQuery.Ask(spec);
            var totalCount = ((IQuery<UberProductSpec, int>) _productQuery).Ask(spec);

            Assert.Equal(1, res.TotalCount);
            Assert.Equal(res.TotalCount, totalCount);
            Assert.Equal(res.Count(), nonPagedRes.Count());
        }

        [Fact]
        public void ProjectionQuery_Ask_Success()
        {
            var spec = new UberProductSpec();
            IQuery<UberProductSpec, IEnumerable<ProductDto>> q = new ProjectionQuery<Product, ProductDto>(_linqProvider, new StaticAutoMapperWrapper());
            var res = q.Ask(spec);
            Assert.Equal(1, res.Count());
        }

        [Fact]
        public void GetQuery_Ask_ResultIsRight()
        {
            var q = new GetByIdQuery<int, Product, ProductDto>(_linqProvider, new StaticAutoMapperWrapper());
            var res = q.Ask(5);
            Assert.Equal(5, res.Id);
        }
    }
}