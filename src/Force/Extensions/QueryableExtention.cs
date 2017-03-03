using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Force.Common;
using Force.Cqrs;
using Force.Ddd;
using Force.Ddd.Entities;
using Force.Ddd.Specifications;

namespace Force.Extensions
{
    public static class QueryableExtention
    {
        public static IQueryable<T> OrderIf<T, TKey>(this IQueryable<T> query, bool condition, bool acs, Expression<Func<T, TKey>> keySelector)
        {
            if (!condition)
            {
                return query;
            }

            return acs
                ? query.OrderBy(keySelector)
                : query.OrderByDescending(keySelector);
        }

        public static IQueryable<T> OrderThenIf<T, TKey>(this IQueryable<T> query, bool condition, bool acs, Expression<Func<T, TKey>> keySelector)
        {
            if (!condition)
                return query;
            return acs
                ? ((IOrderedQueryable<T>)query).ThenBy(keySelector)
                : ((IOrderedQueryable<T>)query).ThenByDescending(keySelector);
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition ? query.Where<T>(predicate) : query;
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicateIfTrue, Expression<Func<T, bool>> predicateIfFalse)
           => condition
                ? query.Where<T>(predicateIfTrue)
                : query.Where<T>(predicateIfFalse);


        public static bool In<T>(this T value, params T[] values)
            =>  values != null && values.Contains(value);

        public static bool In<T>(this T value, IQueryable<T> values)
            => values != null && values.Contains(value);

        public static bool In<T>(this T value, IEnumerable<T> values)
            => values != null && values.Contains(value);

        public static bool NotIn<T>(this T value, params T[] values)
            => values == null || !values.Contains<T>(value);

        public static bool NotIn<T>(this T value, IQueryable<T> values)
            => values == null || !values.Contains<T>(value);

        public static bool NotIn<T>(this T value, IEnumerable<T> values)
            => values == null || !values.Contains<T>(value);

        public static IQueryable<T> Where<T>(this IQueryable<T> source, ILinqSpecification<T> spec)
            where T : class
            => spec.Apply(source);

        public static IQueryable<T> MaybeOrderBy<T>(this IQueryable<T> source, object sort)
        {
            var srt = sort as ILinqOrderBy<T>;
            return srt != null
                ? srt.Apply(source)
                : source;
        }

        public static IQueryable<TDest> ApplyProjectApplyAgain<TSource, TDest>(this IQueryable<TSource> queryable, IProjector projector, object spec)
            where TSource : class
            where TDest : class
            => queryable
            .MaybeWhere(spec)
            .MaybeOrderBy(spec)
            .Project<TDest>(projector)
            .MaybeWhere(spec)
            .MaybeOrderBy(spec);

        public static IQueryable<TDest> ApplyProjectApplyAgainWithoutOrderBy<TSource, TDest>(
            this IQueryable<TSource> queryable, IProjector projector, object spec)
            where TSource : class
            where TDest : class
            => queryable
                .MaybeWhere(spec)
                .Project<TDest>(projector)
                .MaybeWhere(spec);

        public static IQueryable<T> MaybeWhere<T>(this IQueryable<T> source, object spec)
            where T : class
        {
            var specification = spec as ILinqSpecification<T>;
            if (specification != null)
            {
                source = specification.Apply(source);
            }

            var expr = spec as Expression<Func<T, bool>>;
            if (expr != null)
            {
                source = source.Where(expr);
            }

            var exprSpec = spec as ExpressionSpecification<T>;
            if (exprSpec != null)
            {
                source = source.Where(exprSpec.Expression);
            }
            return source;
        }

        public static IQueryable<TDest> Project<TDest>(this IQueryable source, IProjector projector)
            => projector.Project<TDest>(source);

        public static TEntity ById<TEntity>(this ILinqProvider linqProvider, int id)
            where TEntity : class, IHasId<int>
            => linqProvider.Query<TEntity>().ById(id);

        public static TEntity ById<TEntity>(this IQueryable<TEntity> queryable, int id)
            where TEntity : class, IHasId<int>
            => queryable.SingleOrDefault(x => x.Id == id);
    }
}
