using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Force.Ddd;
using Force.Linq.Pagination;

namespace Force.Linq
{
    public static class QueryableExtentions
    {
        #region Left Join

        private class LeftJoinIntermediate<TOuter, TInner>
        {
            public TOuter OneOuter { get; set; }
            public IEnumerable<TInner> ManyInners { get; set; }
        }

        private class Replacer : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParam;
            private readonly Expression _replacement;

            public Replacer(ParameterExpression oldParam, Expression replacement)
            {
                _oldParam = oldParam;
                _replacement = replacement;
            }

            public override Expression Visit(Expression exp)
            {
                if (exp == _oldParam)
                {
                    return _replacement;
                }

                return base.Visit(exp);
            }
        }

        public static IQueryable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(
            this IQueryable<TOuter> outer,
            IQueryable<TInner> inner,
            Expression<Func<TOuter, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector,
            Expression<Func<TOuter, TInner, TResult>> resultSelector)
        {
            var groupJoin = typeof(Queryable).GetMethods()
                .First(m => m.ToString() ==
                            "System.Linq.IQueryable`1[TResult] GroupJoin[TOuter,TInner,TKey,TResult](System.Linq.IQueryable`1[TOuter], System.Collections.Generic.IEnumerable`1[TInner], System.Linq.Expressions.Expression`1[System.Func`2[TOuter,TKey]], System.Linq.Expressions.Expression`1[System.Func`2[TInner,TKey]], System.Linq.Expressions.Expression`1[System.Func`3[TOuter,System.Collections.Generic.IEnumerable`1[TInner],TResult]])")
                .MakeGenericMethod(typeof(TOuter), typeof(TInner), typeof(TKey),
                    typeof(LeftJoinIntermediate<TOuter, TInner>));
            var selectMany = typeof(Queryable).GetMethods()
                .First(m => m.ToString() ==
                            "System.Linq.IQueryable`1[TResult] SelectMany[TSource,TCollection,TResult](System.Linq.IQueryable`1[TSource], System.Linq.Expressions.Expression`1[System.Func`2[TSource,System.Collections.Generic.IEnumerable`1[TCollection]]], System.Linq.Expressions.Expression`1[System.Func`3[TSource,TCollection,TResult]])")
                .MakeGenericMethod(typeof(LeftJoinIntermediate<TOuter, TInner>), typeof(TInner), typeof(TResult));

            var groupJoinResultSelector =
                (Expression<Func<TOuter, IEnumerable<TInner>, LeftJoinIntermediate<TOuter, TInner>>>)
                ((oneOuter, manyInners) => new LeftJoinIntermediate<TOuter, TInner>
                    {OneOuter = oneOuter, ManyInners = manyInners});

            var exprGroupJoin = Expression.Call(groupJoin, outer.Expression, inner.Expression, outerKeySelector,
                innerKeySelector, groupJoinResultSelector);

            var selectManyCollectionSelector =
                (Expression<Func<LeftJoinIntermediate<TOuter, TInner>, IEnumerable<TInner>>>)
                (t => t.ManyInners.DefaultIfEmpty());

            var paramUser = resultSelector.Parameters.First();

            var paramNew = Expression.Parameter(typeof(LeftJoinIntermediate<TOuter, TInner>), "t");
            var propExpr = Expression.Property(paramNew, "OneOuter");

            var selectManyResultSelector = Expression.Lambda(
                new Replacer(paramUser, propExpr).Visit(resultSelector.Body), paramNew,
                resultSelector.Parameters.Skip(1).First());

            var exprSelectMany = Expression.Call(selectMany, exprGroupJoin,
                selectManyCollectionSelector, selectManyResultSelector);

            return outer.Provider.CreateQuery<TResult>(exprSelectMany);
        }

        #endregion

        #region Filter

        public static IQueryable<T> FilterByConventions<T>(this IQueryable<T> queryable, object filter, 
            ComposeKind composeKind = ComposeKind.And)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            var spec = (Spec<T>)SpecBuilder<T>.Build((dynamic) filter, composeKind);
            return spec != null
                ? queryable.Where(spec)
                : queryable;
        }

        public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, IFilter<T> filter)
            => filter.Filter(queryable);
        
        public static IOrderedQueryable<T> FilterAndSort<T, TQuery>(this IQueryable<T> queryable, TQuery query)
            where TQuery : IFilter<T>, ISorter<T>
            => queryable
                .Filter(query)
                .Sort(query);

        public static PagedEnumerable<T> FilterSortAndPaginate<T, TQuery>(this IQueryable<T> queryable, TQuery query)
            where TQuery : IFilter<T>, ISorter<T>, IPaging
            => queryable
                .FilterAndSort(query)
                .ToPagedEnumerable(query);

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
        
        #endregion

        #region Order

        public static IOrderedQueryable<T> OrderById<T>(this IQueryable<T> queryable)
            where T : IHasId
            => queryable.OrderBy(x => x.Id);

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string orderPropertyName)
        {
            try
            {
                return source.OrderBy(ToLambda<T>(orderPropertyName));
            }
            catch (ArgumentException e) when(e.Message.Contains("Instance property") 
                                             && e.Message.Contains("is not defined for type"))
            {
                throw new ArgumentException(
                    $"Order property '{orderPropertyName}' is not defined for type {typeof(T)}", 
                    nameof(orderPropertyName), e);
            }

        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string orderPropertyName)
        {
            try
            {
                return source.OrderByDescending(ToLambda<T>(orderPropertyName));
            }
            catch (ArgumentException e) when (e.Message.Contains("Instance property")
                                              && e.Message.Contains("is not defined for type"))
            {
                throw new ArgumentException(
                    $"Order property '{orderPropertyName}' is not defined for type {typeof(T)}",
                    nameof(orderPropertyName), e);
            }
        }

        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);    
        }
        #endregion
        
        #region ById
    
        public static IQueryable<TEntity> ById<TKey, TEntity>(this IQueryable<TEntity> queryable, TKey id)
            where TKey : IEquatable<TKey>
            where TEntity : class, IHasId<TKey>
            => queryable.Where(x => x.Id.Equals(id));

        public static TEntity FirstOrDefaultById<TKey, TEntity>(this IQueryable<TEntity> queryable, TKey id)
            where TKey : IEquatable<TKey>
            where TEntity : class, IHasId<TKey>
            => queryable.FirstOrDefault(x => x.Id.Equals(id));
        
        public static TProjection FirstOrDefaultById<TKey, TEntity, TProjection>(this IQueryable<TEntity> queryable, 
            TKey id, Expression<Func<TEntity, TProjection>> projectionExpression)
            where TKey : IEquatable<TKey>
            where TEntity : class, IHasId<TKey>
            where TProjection : class, IHasId<TKey>
            => queryable
                .Select(projectionExpression)
                .FirstOrDefault(x => x.Id.Equals(id));
        
        #endregion
        
    }


}
