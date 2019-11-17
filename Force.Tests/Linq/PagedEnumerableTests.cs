using System.Collections;
using Force.Linq.Pagination;
using Xunit;

namespace Force.Tests.Linq
{
    public class PagedEnumerableTests
    {
        [Fact]
        public void Ctr()
        {
            var arr = new[] {"1", "2", "3"};
            var pe = new PagedEnumerable<string>(arr, arr.Length);
            var enm = pe.GetEnumerator();
            var enm2 = ((IEnumerable) pe).GetEnumerator();
            var t = pe.Total;
        }
    }
}