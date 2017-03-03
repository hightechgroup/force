using CostEffectiveCode.Ddd.Entities;
using CostEffectiveCode.Tests.Stubs;
using Xunit;

namespace CostEffectiveCode.Tests
{
    public class HasIdTests
    {
        [Fact]
        public void HasId_IsNew_True()
        {
            var hasId = new HasId();
            Assert.True(hasId.IsNew());
        }

        [Fact]
        public void HasId_IsNew_False()
        {
            var hasId = new HasId()
            {
                Id = 100500
            };

            Assert.False(hasId.IsNew());
        }

        [Fact]
        public void HasId_ObjectAndTypeId_AreEqual()
        {
            var hasId = new HasId()
            {
                Id = 100500
            };

            Assert.True(((IHasId) hasId).Id?.Equals(100500) == true);
        }
    }
}
