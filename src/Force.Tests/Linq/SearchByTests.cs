using Force.Linq;
using Xunit;

namespace Force.Tests.Linq
{
    public class SearchByTests
    {
        [Theory]
        [InlineData(SearchKind.Contains)]
        [InlineData(SearchKind.StartsWith)]
        public void New(SearchKind searchKind)
        {
            var sba = new SearchByAttribute(searchKind);
            Assert.Equal(searchKind, sba.SearchKind);
        }
    }
}