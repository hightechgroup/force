using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Force.Ddd;
using Force.Ddd.Pagination;

namespace Force.Extensions
{
    public static class QueryableExtention
    {
        public static IOrderedQueryable<T> OrderByIdIfNotOrdered<T>(this IQueryable<T> query)
            where T : IHasId
            => (query as IOrderedQueryable<T>) ?? query.OrderBy(x => x.Id);

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
            Expression<Func<T, bool>> predicateIfTrue, Expression<Func<T, bool>> predicateIfFalse)
            => query.WhereIf(obj != null, predicateIfTrue, predicateIfFalse);

        public static bool In<T>(this T value, params T[] values)
            =>  values != null && values.Contains(value);

        public static bool In<T>(this T value, IQueryable<T> values)
            => values != null && values.Contains(value);

        public static bool In<T>(this T value, IEnumerable<T> values)
            => values != null && values.Contains(value);

        public static bool NotIn<T>(this T value, params T[] values)
            => values == null || !values.Contains(value);

        public static bool NotIn<T>(this T value, IQueryable<T> values)
            => values == null || !values.Contains(value);

        public static bool NotIn<T>(this T value, IEnumerable<T> values)
            => values == null || !values.Contains(value);

        public static IQueryable<T> Where<T>(this IQueryable<T> source, IQueryableFilter<T> spec)
            where T : class
            => spec.Apply(source);

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, IQueryableOrder<T> spec)
            where T : class
            => spec.Apply(source);

        public static IQueryable<T> MaybeOrderBy<T>(this IQueryable<T> source, object sort)
        {
            var srt = sort as IQueryableOrder<T>;
            return srt != null
                ? srt.Apply(source)
                : source;
        }

        public static IQueryable<TSource> Apply<TSource>(this IQueryable<TSource> queryable, object spec)
            where TSource : class
            => queryable
            .MaybeWhere(spec)
            .MaybeOrderBy(spec);

        public static IQueryable<T> MaybeWhere<T>(this IQueryable<T> source, object spec)
            where T : class
        {
            var specification = spec as IQueryableFilter<T>;
            if (specification != null)
            {
                source = specification.Apply(source);
            }

            var expr = spec as Expression<Func<T, bool>>;
            if (expr != null)
            {
                source = source.Where(expr);
            }

            return source;
        }

        public static TEntity ById<TKey, TEntity>(this IQueryable<TEntity> queryable, TKey id)
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
            where TEntity : class, IHasId<TKey>
            => queryable.SingleOrDefault(x => x.Id.Equals(id));

        public static TProjection ById<TKey, TEntity, TProjection>(this IQueryable<TEntity> queryable, TKey id, Expression<Func<TEntity, TProjection>> projectionExpression)
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
            where TEntity : class, IHasId<TKey>
            where TProjection : class, IHasId<TKey>
            => queryable.Select(projectionExpression).SingleOrDefault(x => x.Id.Equals(id));
    }
}
