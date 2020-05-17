using System.Collections;
using System.Linq;
using Force.Linq.Pagination;
using Xunit;

namespace Force.Tests.Linq
{
    public class PagedEnumerableTests
    {
        [Fact]
        public void New()
        {
            var arr = new[] {"1", "2", "3"};
            var pe = new PagedEnumerable<string>(arr, arr.Length);
            var enm = pe.GetEnumerator();
            var enm2 = ((IEnumerable) pe).GetEnumerator();
            var t = pe.Total;
            
            Assert.Equal(arr.Length, t);

            var count = 0;
            while (enm.MoveNext())
            {
                Assert.Contains(enm.Current, arr);
                count++;
            }
            
            Assert.Equal(arr.Length, count);
            
            count = 0;
            while (enm2.MoveNext())
            {
                Assert.Contains(enm2.Current, arr);
                count++;
            }
            
            Assert.Equal(arr.Length, count);
        }
    }
}