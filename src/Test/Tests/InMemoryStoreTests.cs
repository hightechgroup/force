using System.Linq;
using CostEffectiveCode.Ddd;
using CostEffectiveCode.Extensions;
using Xunit;

namespace CostEffectiveCode.Tests
{
    public class InMemoryStoreTests
    {
        private InMemoryStore _store;

        public InMemoryStoreTests()
        {
            _store = new InMemoryStore();
        }

        [Fact]
        public void AddRemove()
        {
            var c = new Category(5, "super"){Id = 1};
            _store.Add(c);
            c = _store.Query<Category>().ById(1);

            Assert.Equal("super", c.Name);
            Assert.Equal(5, c.Rating);

			var a = _store.Query<Category>().Where(x => x.Rating > 5).ToArray();

            c = (Category)_store.Find(typeof(Category), 1);
            Assert.Equal("super", c.Name);
            Assert.Equal(5, c.Rating);

            _store.Delete(c);

            Assert.False(_store.Query<Category>().Any());
        }
    }
}