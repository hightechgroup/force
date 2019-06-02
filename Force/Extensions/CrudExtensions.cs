using System;
using System.Linq;
using System.Linq.Expressions;
using Force.Ddd;

namespace Force.Extensions
{
    public static class CrudExtensions
    {
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

        public static TKey Create<TKey, TDto, TEntity>(this IUnitOfWork uow,
            TDto dto, Func<TDto, IUnitOfWork, TEntity> mapper)
            where TEntity : class, IHasId<TKey>
            where TKey : IEquatable<TKey>
        {
            var entity = mapper(dto, uow);
            uow.Add(entity);
            uow.Commit();
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

            uow.Commit();
            return entity.Id;
        }

        public static void Update<TKey, TEntity, TDto>(this IUnitOfWork uow, TKey id,
            TDto dto, Func<TDto, TEntity, TEntity> mapper)
            where TEntity : class, IHasId<TKey>
            where TKey : IEquatable<TKey>
        {
            var entity = uow.Find<TEntity>(id);
            mapper(dto, entity);
            uow.Commit();
        }

        public static void Update<TKey, TEntity, TDto>(this IUnitOfWork uow, TEntity entity,
            TDto dto, Func<TDto, TEntity, TEntity> mapper)
            where TEntity : class, IHasId<TKey>
            where TKey : IEquatable<TKey>
        {
            mapper(dto, entity);
            uow.Commit();
        }
    }
}