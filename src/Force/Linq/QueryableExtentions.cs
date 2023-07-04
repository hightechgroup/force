using System;
using System.Linq;
using System.Linq.Expressions;
using Force.Ddd;

namespace Force.Linq
{
    public static class QueryableExtentions
    {
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
