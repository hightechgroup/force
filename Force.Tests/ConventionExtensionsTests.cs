using System.Linq;
using Force.Ddd;
using Force.Meta;
using Xunit;

namespace Force.Tests
{
    public class DummyEntity : HasIdBase<int>
    {
        public string Name { get; set; }

        public string DetailsValue { get; set; }
    }

    public class DummyParams: HasIdBase<int>
    {
        public string Name { get; set; }
                
    }
    
    public class ConventionExtensionsTests
    {
        [Fact]
        public void A()
        {
            var pars = new DummyParams()
            {
                Name = "Some Name"
            };

            var entity = new DummyEntity()
            {
                Name = "Name",
                DetailsValue = "Details"
            };

            var q = new[] {entity}.AsQueryable();
            var data1 = Conventions<DummyEntity>.Filter(q, pars).ToList();
            var abc = q.AutoFilter(pars).ToArray();
            Assert.Equal(0, data1.Count);

            pars.Name = "Name";
            var data2 = Conventions<DummyEntity>.Filter(q, pars).ToList();
            Assert.Equal(1, data2.Count);

        }
    }
}