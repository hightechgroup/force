using System;
using System.Linq;
using System.Reflection;
using Force.Cqrs;
using Force.Cqrs.Queries;
using Force.Ddd.Entities;
using Force.Ddd.Pagination;

namespace Force.Components
{
    public static class RestTypes
    {
        public static Type GetEntityType(this Type type)
        {
            try
            {
                return type
                    .GetTypeInfo()
                    .GetCustomAttributes<ProjectionAttribute>()
                    .Single()
                    .EntityType;
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException($"Type {type.Name} is not marked with [ProjectionAttribute]", e);
            }
        }

        public static Type Get<TKey, TProjection>()
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
            where TProjection : IHasId<TKey>
            => typeof(GetByIdQuery<,,>).MakeGenericType(
                typeof(TKey),
                typeof(TProjection).GetEntityType(),
                typeof(TProjection));

        public static Type Paged<TSpec, TProjection>()
            where TSpec : class, ILinqOrderBy<TProjection>
            where TProjection : class
            => typeof(PagedQuery<,>).MakeGenericType(typeof(TSpec), typeof(TProjection));


        public static TProjection GetById<TKey, TEntity, TProjection>(
            this IHasGetByIdQuery<TKey, TEntity, TProjection> hasQuery, TKey id)
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
            where TEntity : class, IHasId<TKey>
            where TProjection : IHasId<TKey>
            => hasQuery.GetByIdQuery.Ask(id);

        public static IPagedEnumerable<TProjection> GetPaged<TSpec, TProjection>(
            this IHasPagedQuery<TSpec, TProjection> hasPagedQuery, TSpec spec)
            where TSpec : class, IPaging<TProjection, int>, IHasId
            where TProjection : class
            => hasPagedQuery.PagedQuery.Ask(spec);
    }

    public interface IHasGetByIdQuery<TKey, TEntity, TProjection>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        where TEntity : class, IHasId<TKey>
        where TProjection : IHasId<TKey>
    {
        GetByIdQuery<TKey, TEntity, TProjection> GetByIdQuery { get; }
    }

    public interface IHasPagedQuery<TSpec, TProjection>
        where TSpec : class, IPaging<TProjection, int>, IHasId
        where TProjection : class
    {
        PagedQuery<TSpec, TProjection, int> PagedQuery { get; }
    }
}