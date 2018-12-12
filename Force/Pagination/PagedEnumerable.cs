using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Force.Ddd.Pagination
{
    public class PagedEnumerable<T>: IEnumerable<T>
    {
        private IEnumerable<T> _enumerable;
        
        public PagedEnumerable(IEnumerable<T> enumerable, long total)
        {
            Total = total;
            _enumerable = enumerable;

        }

        public long Total { get; }

        public IEnumerator<T> GetEnumerator()
            => _enumerable.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _enumerable.GetEnumerator();
    }
}