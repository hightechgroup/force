using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using CostEffectiveCode.Components;
using CostEffectiveCode.Cqrs;
using CostEffectiveCode.Cqrs.Commands;
using CostEffectiveCode.Cqrs.Queries;
using CostEffectiveCode.Ddd.Pagination;
using CostEffectiveCode.Ddd.Specifications;
using Xunit;

namespace CostEffectiveCode.Tests
{
    public class AutoRegistrationTests
    {
        [Fact]
        public void AutoRegistration_MainComponentsSetUp()
        {
            var sw = new Stopwatch();
            sw.Start();
            var assembly = GetType().GetTypeInfo().Assembly;
            var res = new AutoRegistration().GetComponentMap(t => t == typeof(TestDependentObject), assembly, t => true, assembly);
            sw.Stop();

            Assert.Equal(
                typeof(ProjectionQuery<Product, ProductDto>),
                res[typeof(IQuery<object, IEnumerable<ProductDto>>)]);

            Assert.Equal(
                typeof(PagedQuery<Product, ProductDto, int>),
                res[typeof(IQuery<IdPaging<ProductDto>, IPagedEnumerable<ProductDto>>)]);

            Assert.Equal(
                typeof(GetByIdQuery<int, Product, ProductDto>),
                res[typeof(IQuery<int, ProductDto>)]);

            Assert.Equal(
                typeof(CreateOrUpdateEntityHandler<int, ProductDto, Product>),
                res[typeof(IHandler<ProductDto, int>)]);

            Assert.True(sw.ElapsedMilliseconds < 50, $"Elapsed ms: {sw.ElapsedMilliseconds}");
        }
    }
}
