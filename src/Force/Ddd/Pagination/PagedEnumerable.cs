using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Force.Ddd.Pagination
{
    [PublicAPI]
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

    [PublicAPI]
    public static class PagedEnumerableExtensions
    {
        public static IOrderedQueryable<TEntity> OrderBy<TEntity, TKey>(this IQueryable<TEntity> queryable
            , OrderBy<TEntity, TKey> orderBy)
            where TEntity : class
            => orderBy.SortOrder == SortOrder.Asc
                ? queryable.OrderBy(orderBy.Expression)
                : queryable.OrderByDescending(orderBy.Expression);

        public static IOrderedQueryable<TEntity> ThenBy<TEntity, TKey>(this IOrderedQueryable<TEntity> queryable
            , OrderBy<TEntity, TKey> orderBy)
            where TEntity : class
            => orderBy.SortOrder == SortOrder.Asc
                ? queryable.ThenBy(orderBy.Expression)
                : queryable.ThenByDescending(orderBy.Expression);

        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, IPaging  paging) => queryable
            .Skip((paging.Page - 1) * paging.Take)
            .Take(paging.Take);

        public static IQueryable<T> Paginate<T, TKey>(this IQueryable<T> queryable, IPaging<T, TKey> paging)
            where T : class
        {
            if(!paging.OrderBy.Any())
            {
                throw new ArgumentException("OrderBy can't be null or empty", nameof(paging));
            }

            var ordered = queryable.OrderBy(paging.OrderBy.First());
            ordered = paging.OrderBy.Skip(1).Aggregate(ordered, (current, order) => current.ThenBy(order));

            return ordered
                .Skip((paging.Page - 1) * paging.Take)
                .Take(paging.Take);
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> queryable, ILinqOrderBy<T> orderBy)
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
