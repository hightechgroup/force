using System.Collections.Generic;
using System.Linq;

namespace Force.Ddd.Pagination
{
    public sealed class PagedResponse<T>
    {
        public PagedResponse(IOrderedQueryable<T> queryable, IPaging paging)
        {
            Total = queryable.Count();
            TotalPages = Total / paging.Take;
            Items = queryable.Paginate(paging).ToList();
        }
        
        public PagedResponse(IEnumerable<T> items, long total, int totalPages)
        {
            Total = total;
            TotalPages = totalPages;
            Items = items.ToArray();
        }

        public long Total { get; }
        
        public long TotalPages { get; }

        public IEnumerable<T> Items { get; }
    }
}