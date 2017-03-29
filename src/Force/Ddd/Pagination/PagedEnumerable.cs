using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Force.Ddd.Pagination
{
    public interface IPagedEnumerable<out T> : IEnumerable<T>
    {
        /// <summary>
        /// Total number of entries across all pages.
        /// </summary>
        long TotalCount { get; }
    }

    public class PagedEnumerable<T> : IPagedEnumerable<T>
    {
        private readonly IEnumerable<T> _inner;
        private readonly int _totalCount;

        public PagedEnumerable(IEnumerable<T> inner, int totalCount)
        {
            _inner = inner;
            _totalCount = totalCount;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public long TotalCount => _totalCount;
    }

    public static class PagedEnumerableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, IPaging  paging) => queryable
            .Skip((paging.Page - 1) * paging.Take)
            .Take(paging.Take);

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> queryable, IQueryableOrderBy<T> orderBy)
            where T : class
            => orderBy.Apply(queryable);

        public static IPagedEnumerable<T> ToPagedEnumerable<T>(this IQueryable<T> queryable,
            IPaging paging)
            where T : class
            => From(queryable.Paginate(paging).ToArray(), queryable.Count());

        public static IPagedEnumerable<T> From<T>(IEnumerable<T> inner, int totalCount)
            =>  new PagedEnumerable<T>(inner, totalCount);

        public static IPagedEnumerable<T> Empty<T>()
            =>  From(Enumerable.Empty<T>(), 0);
    }

}
