using System;
using System.Linq;
using System.Linq.Expressions;
using Force.Common;
using Force.Ddd;
using Force.Ddd.Entities;
using Force.Ddd.Pagination;
using Force.Extensions;

namespace Force
{
    public static class Rest
    {
        public static TProjection ById<TKey, TEntity, TProjection>(this ILinqProvider linqProvider, TKey id,
            Func<TEntity, TProjection> mapper)
            where TKey : class, IHasId<int>, IComparable, IComparable<TKey>, IEquatable<TKey>
            where TProjection : class
            where TEntity : class, IHasId<TKey> =>
            linqProvider.ById<TKey, TEntity>(id).PipeTo(mapper);

        public static TProjection ById<TKey, TEntity, TProjection>(this ILinqProvider linqProvider, TKey id,
            Expression<Func<TEntity, TProjection>> projectior)
            where TKey : class, IComparable, IComparable<TKey>, IEquatable<TKey>
            where TProjection : class, IHasId<TKey>
            where TEntity : class, IHasId<TKey>
            => linqProvider.Query<TEntity>().Select(projectior).ById(id);


        public static TProjection ById<TKey, TEntity, TProjection>(this ILinqProvider linqProvider, TKey id,
            IProjector projector)
            where TKey : class, IHasId<int>, IComparable, IComparable<TKey>, IEquatable<TKey>
            where TProjection : class, IHasId<TKey>
            where TEntity : class, IHasId<TKey> =>
            linqProvider.Query<TEntity>().Project<TProjection>(projector).ById(id);

        public static IPagedEnumerable<TDest> ListPaged<TEntity, TDest>(this ILinqProvider linqProvider
            , IPaging spec , Expression<Func<TEntity, TDest>> projectionExpression)
            where TEntity : class, IHasId where TDest : class
            => linqProvider
                .Query<TEntity>()
                .MaybeWhere(spec)
                .Select(projectionExpression)
                .MaybeWhere(spec)
                .MaybeOrderBy(spec)
                .ToPagedEnumerable(spec);

        public static IPagedEnumerable<TDest> ListPaged<TEntity, TDest>(this ILinqProvider linqProvider
            , IPaging spec, IProjector projector)
            where TEntity : class, IHasId where TDest : class
            => linqProvider
                .Query<TEntity>()
                .MaybeWhere(spec)
                .Project<TDest>(projector)
                .MaybeWhere(spec)
                .MaybeOrderBy(spec)
                .ToPagedEnumerable(spec);

        public static TKey Create<TKey, TEntity, TCommand>(this IUnitOfWork uow
            , TCommand command, Func<TCommand, TEntity> mapper)
            where TEntity : class, IHasId<TKey>
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            var entity = mapper(command);
            uow.Add(entity);
            uow.Commit();
            return entity.Id;
        }

        public static void Update<TKey, TEntity, TCommand>(this IUnitOfWork uow, TKey id
            , TCommand command, Func<TCommand, TEntity, TEntity> mapper)
            where TEntity : class, IHasId<TKey>
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            var entity = uow.Find<TEntity>(id);
            entity = mapper(command, entity);
            uow.Commit();
        }
    }
}