using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Force.Ddd;
using Force.Ddd.Pagination;

namespace Force.Extensions
{
    public static class QueryableExtentions
    {       
        public static IQueryable<T> TryFilter<T>(this IQueryable<T> queryable, object maybeFilter)
        {
            if (maybeFilter is IFilter<T> filter)
            {
                queryable = queryable.Where(filter.Spec);
            }

            return queryable;
        }
        
        public static IOrderedQueryable<T> FilterAndSort<T, TQuery>(this IQueryable<T> queryable, TQuery query)
            where TQuery : IFilter<T>, ISorter<T>
            => queryable
                .Filter(query)
                .Sort(query);

        public static PagedResponse<T> FilterSortAndPaginate<T, TQuery>(this IQueryable<T> queryable, TQuery query)
            where TQuery : IFilter<T>, ISorter<T>, IPaging
            => queryable
                .FilterAndSort(query)
                .ToPagedResponse(query);

        public static IOrderedQueryable<T> OrderByFirstProperty<T>(this IQueryable<T> queryable)
            => typeof(IHasId).IsAssignableFrom(typeof(T))
                ? (IOrderedQueryable<T>) ((dynamic) queryable).OrderById()
                : Conventions<T>.Sort(queryable, typeof(T).GetProperties().First().Name);
        
        public static IOrderedQueryable<T> OrderById<T>(this IQueryable<T> queryable)
            where T : IHasId
            => (queryable as IOrderedQueryable<T>) ?? queryable.OrderBy(x => x.Id);

        public static IQueryable<T> OrderIf<T, TKey>(this IQueryable<T> query, bool condition,
            bool acs, Expression<Func<T, TKey>> keySelector)
        {
            if (!condition)
            {
                return query;
            }

            return acs
                ? query.OrderBy(keySelector)
                : query.OrderByDescending(keySelector);
        }

        public static IQueryable<T> OrderThenIf<T, TKey>(this IQueryable<T> query, bool condition,
            bool acs, Expression<Func<T, TKey>> keySelector)
        {
            if (!condition)
                return query;
            return acs
                ? ((IOrderedQueryable<T>)query).ThenBy(keySelector)
                : ((IOrderedQueryable<T>)query).ThenByDescending(keySelector);
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition,
            Expression<Func<T, bool>> predicate)
        {
            return condition ? query.Where(predicate) : query;
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition,
            Expression<Func<T, bool>> predicateIfTrue, Expression<Func<T, bool>> predicateIfFalse)
           => condition
                ? query.Where(predicateIfTrue)
                : query.Where(predicateIfFalse);

        public static IQueryable<T> WhereIfNotNull<T>(this IQueryable<T> query, object obj,
            Expression<Func<T, bool>> predicateIfTrue)
            => query.WhereIf(obj != null, predicateIfTrue);
       

        public static TEntity ById<TKey, TEntity>(this IQueryable<TEntity> queryable, TKey id)
            where TKey : IEquatable<TKey>
            where TEntity : class, IHasId<TKey>
            => queryable.FirstOrDefault(x => x.Id.Equals(id));

        public static TProjection ById<TKey, TEntity, TProjection>(this IQueryable<TEntity> queryable, TKey id, 
            Expression<Func<TEntity, TProjection>> projectionExpression)
            where TKey : IEquatable<TKey>
            where TEntity : class, IHasId<TKey>
            where TProjection : class, IHasId<TKey>
            => queryable
                .Select(projectionExpression)
                .FirstOrDefault(x => x.Id.Equals(id));
    }
}
