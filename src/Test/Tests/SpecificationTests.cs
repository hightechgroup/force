using CostEffectiveCode.Ddd.Specifications;
using Xunit;

namespace CostEffectiveCode.Tests
{
    public class SpecificationTests
    {
        private Product _product = new Product(new Category(), "123", 123) {Id = 10};

        [Fact]
        public void IdSpecification_IsSatisfiedBy_Success()
        {
            var spec = new IdSpecification<int, Product>(10);
            Assert.True(spec.IsSatisfiedBy(_product));
        }

        public void ExpressionSpecification_IsSatisfiedBy_Success()
        {
            var spec = new ExpressionSpecification<Product>(x => x.Id == 10);
            Assert.True(spec.IsSatisfiedBy(_product));
        }
    }
}
