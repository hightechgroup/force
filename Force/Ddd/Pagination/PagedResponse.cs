using System.Collections.Generic;
using System.Linq;

namespace Force.Ddd.Pagination
{
    public sealed class PagedResponse<T>
    {
        public PagedResponse(IEnumerable<T> items, long total)
        {
            Total = total;
            Items = items.ToArray();
        }

        public long Total { get; }

        public T[] Items { get; }
    }
}