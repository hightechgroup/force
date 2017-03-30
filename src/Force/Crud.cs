using System;
using System.Linq;
using System.Linq.Expressions;
using Force.Ddd;
using Force.Ddd.Pagination;
using Force.Extensions;

namespace Force
{
    public static class Crud
    {
        public static long Count<T>(this IQueryable<T> query, IQueryableSpecification<T> spec) where T : class
            => query.Apply(spec).Count();

        public static TProjection ById<TKey, TEntity, TProjection>(this IQueryableProvider queryableProvider, TKey id,
            Func<TEntity, TProjection> mapper)
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
            where TProjection : class
            where TEntity : class, IHasId<TKey> =>
            queryableProvider.ById<TKey, TEntity>(id).PipeTo(mapper);

        public static TProjection ProjectById<TKey, TEntity, TProjection>(this IQueryableProvider queryableProvider, TKey id,
            Expression<Func<TEntity, TProjection>> projector)
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
            where TProjection : class, IHasId<TKey>
            where TEntity : class, IHasId<TKey>
            => queryableProvider.Query<TEntity>().Select(projector).ById(id);

        public static IPagedEnumerable<TDest> Paged<TEntity, TDest>(this IQueryableProvider queryableProvider,
            IPaging spec , Expression<Func<TEntity, TDest>> projectionExpression)
            where TEntity : class, IHasId where TDest : class
            => queryableProvider
                .Query<TEntity>()
                .MaybeWhere(spec)
                .Select(projectionExpression)
                .MaybeWhere(spec)
                .MaybeOrderBy(spec)
                .ToPagedEnumerable(spec);

        public static TKey Create<TKey, TDto, TEntity>(this IUnitOfWork uow,
            TDto dto, Func<TDto, IUnitOfWork, TEntity> mapper)
            where TEntity : class, IHasId<TKey>
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            var entity = mapper(dto, uow);
            uow.Add(entity);
            uow.Commit();
            return entity.Id;
        }

        public static TKey CreateOrUpdate<TKey, TDto, TEntity>(this IUnitOfWork uow,
            TDto dto, Func<TDto, TEntity, TEntity> mapper)
            where TEntity : class, IHasId<TKey>, new()
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            var entity = (dto as IHasId<TKey>)?.Id.PipeTo(x => uow.Find<TEntity>(x)) ?? new TEntity();
            if (entity.IsNew())
            {
                uow.Add(entity);
            }

            uow.Commit();
            return entity.Id;
        }

        public static void Update<TKey, TEntity, TDto>(this IUnitOfWork uow, TKey id,
            TDto dto, Func<TDto, TEntity, TEntity> mapper)
            where TEntity : class, IHasId<TKey>
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            var entity = uow.Find<TEntity>(id);
            mapper(dto, entity);
            uow.Commit();
        }

        public static void Update<TKey, TEntity, TDto>(this IUnitOfWork uow, TEntity entity,
            TDto dto, Func<TDto, TEntity, TEntity> mapper)
            where TEntity : class, IHasId<TKey>
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            mapper(dto, entity);
            uow.Commit();
        }
    }
}