using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Force.Ddd;
using Force.Ddd.Pagination;
using Force.Ddd.Specifications;
using Force.Extensions;

namespace Force.AutoMapper
{
    public static class Extensions
    {
        public static TProjection ById<TKey, TEntity, TProjection>(this IQueryableProvider queryableProvider, TKey id)
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
            where TProjection : class, IHasId<TKey>
            where TEntity : class, IHasId<TKey> =>
            queryableProvider.Query<TEntity>().ProjectTo<TProjection>().ById(id);

        public static IQueryable<TDest> ApplyProjectApplyAgain<TSource, TDest>(this IQueryable<TSource> queryable, object spec)
            where TSource : class
            where TDest : class
            => queryable
                .MaybeWhere(spec)
                .MaybeOrderBy(spec)
                .ProjectTo<TDest>()
                .MaybeWhere(spec)
                .MaybeOrderBy(spec);

        public static IQueryable<TDest> ApplyProjectApplyAgainWithoutOrderBy<TSource, TDest>(
            this IQueryable<TSource> queryable, object spec)
            where TSource : class
            where TDest : class
            => queryable
                .MaybeWhere(spec)
                .ProjectTo<TDest>()
                .MaybeWhere(spec);

        public static IPagedEnumerable<TDest> Paged<TEntity, TDest>(this IQueryableProvider queryableProvider,
            IPaging spec)
            where TEntity : class, IHasId where TDest : class
            => queryableProvider
                .Query<TEntity>()
                .MaybeWhere(spec)
                .ProjectTo<TDest>()
                .MaybeWhere(spec)
                .MaybeOrderBy(spec)
                .ToPagedEnumerable(spec);

        public static IPagedEnumerable<TDest> Paged<TEntity, TDest>(this IQueryableProvider queryableProvider,
            IPaging paging, IQueryableOrderBy<TDest> queryableOrderBy, IQueryableSpecification<TEntity> entitySpec = null,
            IQueryableSpecification<TDest> destSpec = null)
            where TEntity : class, IHasId where TDest : class
            => queryableProvider
                .Query<TEntity>()
                .EitherOrSelf(entitySpec, x => x.Where(entitySpec))
                .ProjectTo<TDest>()
                .EitherOrSelf(destSpec, x => x.Where(destSpec))
                .OrderBy(queryableOrderBy)
                .ToPagedEnumerable(paging);

        public static TKey Create<TKey, TCommand, TEntity>(this IUnitOfWork uow,
            TCommand command, IMapper mapper)
            where TEntity : class, IHasId<TKey>
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            var entity = mapper.Map<TEntity>(command);
            uow.Add(entity);
            uow.Commit();
            return entity.Id;
        }

        public static void Update<TKey, TEntity, TCommand>(this IUnitOfWork uow, TKey id,
            TCommand command, IMapper mapper)
            where TEntity : class, IHasId<TKey>
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            var entity = uow.Find<TEntity>(id);
            mapper.Map(command, entity);
            uow.Commit();
        }

        public static TDest Map<TDest>(this object obj) => Mapper.Map<TDest>(obj);
    }
}