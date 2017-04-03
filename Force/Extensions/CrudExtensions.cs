using System;
using System.Linq;
using System.Linq.Expressions;
using Force.Ddd;
using Force.Ddd.Pagination;

namespace Force.Extensions
{
    public static class CrudExtensions
    {
        public static long Count<T>(this IQueryable<T> query, IQueryableFilter<T> spec) where T : class
            => query.Apply(spec).Count();

        public static TProjection ById<TKey, TEntity, TProjection>(this IQueryable<TEntity> query, TKey id,
            Func<TEntity, TProjection> mapper)
            where TKey : IEquatable<TKey>
            where TProjection : class
            where TEntity : class, IHasId<TKey> =>
            query.ById(id).PipeTo(mapper);

        public static TProjection ProjectById<TKey, TEntity, TProjection>(this IQueryable<TEntity> query, TKey id,
            Expression<Func<TEntity, TProjection>> projector)
            where TKey : IEquatable<TKey>
            where TProjection : class, IHasId<TKey>
            where TEntity : class, IHasId<TKey>
            => query.Select(projector).ById(id);

        public static IPagedEnumerable<TDest> Paged<TEntity, TDest>(this IQueryable<TEntity> query,
            IPaging spec , Expression<Func<TEntity, TDest>> projectionExpression)
            where TEntity : class, IHasId
            where TDest : class, IHasId
            => query
                .MaybeWhere(spec)
                .Select(projectionExpression)
                .MaybeWhere(spec)
                .MaybeOrderBy(spec)
                .OrderByIdIfNotOrdered()
                .ToPagedEnumerable(spec);

        public static TKey Create<TKey, TDto, TEntity>(this IUnitOfWork uow,
            TDto dto, Func<TDto, IUnitOfWork, TEntity> mapper)
            where TEntity : class, IHasId<TKey>
            where TKey : IEquatable<TKey>
        {
            var entity = mapper(dto, uow);
            uow.Add(entity);
            uow.SaveChanges();
            return entity.Id;
        }

        public static TKey CreateOrUpdate<TKey, TDto, TEntity>(this IUnitOfWork uow,
            TDto dto, Func<TDto, TEntity, TEntity> mapper)
            where TEntity : class, IHasId<TKey>, new()
            where TKey : IEquatable<TKey>
        {
            var entity = (dto as IHasId<TKey>)?.Id.PipeTo(x => uow.Find<TEntity>(x)) ?? new TEntity();
            if (entity.IsNew())
            {
                uow.Add(entity);
            }

            uow.SaveChanges();
            return entity.Id;
        }

        public static void Update<TKey, TEntity, TDto>(this IUnitOfWork uow, TKey id,
            TDto dto, Func<TDto, TEntity, TEntity> mapper)
            where TEntity : class, IHasId<TKey>
            where TKey : IEquatable<TKey>
        {
            var entity = uow.Find<TEntity>(id);
            mapper(dto, entity);
            uow.SaveChanges();
        }

        public static void Update<TKey, TEntity, TDto>(this IUnitOfWork uow, TEntity entity,
            TDto dto, Func<TDto, TEntity, TEntity> mapper)
            where TEntity : class, IHasId<TKey>
            where TKey : IEquatable<TKey>
        {
            mapper(dto, entity);
            uow.SaveChanges();
        }
    }
}