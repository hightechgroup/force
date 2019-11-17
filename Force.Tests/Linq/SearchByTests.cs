using Force.Linq;
using Xunit;

namespace Force.Tests.Linq
{
    public class SearchByTests
    {
        [Fact]
        public void A()
        {
            var sba = new SearchByAttribute(SearchKind.Contains);
            var k = sba.SearchKind;
        }
    }
}