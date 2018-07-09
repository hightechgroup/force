using System.Linq;

namespace Force.Ddd.Pagination
{
    public interface IPaging
    {
        int Page { get; }

        int Take { get; }
    }

    public static class PagingExtensions
    {
        public static IOrderedQueryable<T> Paginate<T>(this IOrderedQueryable<T> queryable, IPaging  paging) 
            => (IOrderedQueryable<T>)queryable
            .Skip((paging.Page - 1) * paging.Take)
            .Take(paging.Take)            ;
        
        public static IOrderedQueryable<T> Paginate<T>(this IOrderedQueryable<T> queryable, int page, int take)
            => (IOrderedQueryable<T>)queryable
            .Skip((page - 1) * take)
            .Take(take);        
    }
}