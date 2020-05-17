using System.Linq;

namespace Force.Linq.Pagination
{
    public interface IPaging
    {
        int Page { get; }

        int Take { get; }
    }

    public static class PagingExtensions
    {
        public static IOrderedQueryable<T> Paginate<T>(this IOrderedQueryable<T> queryable, IPaging paging) 
            => (IOrderedQueryable<T>)queryable
            .Skip((paging.Page - 1) * paging.Take)
            .Take(paging.Take);

        public static PagedEnumerable<T> ToPagedEnumerable<T>(this IOrderedQueryable<T> queryable, IPaging paging)
            => new PagedEnumerable<T>(queryable, paging);
    }
}