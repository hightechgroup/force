using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Force.Ddd;
using Force.Ddd.Pagination;
using Force.Extensions;

namespace Force.AutoMapper
{
    public static class Extensions
    {
        public static TProjection ProjectById<TKey, TEntity, TProjection>(this IQueryableProvider queryableProvider, TKey id)
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
            where TEntity : class, IHasId
            where TDest : class, IHasId => queryableProvider
                .Query<TEntity>()
                .MaybeWhere(spec)
                .ProjectTo<TDest>()
                .MaybeWhere(spec)
                .MaybeOrderBy(spec)
                .OrderByIdIfNotOrdered()
                .ToPagedEnumerable(spec);

        public static IPagedEnumerable<TDest> Paged<TEntity, TDest>(this IQueryableProvider queryableProvider,
            IPaging paging, IQueryableOrder<TDest> queryableOrder, IQueryableFilter<TEntity> entitySpec = null,
            IQueryableFilter<TDest> destSpec = null)
            where TEntity : class, IHasId where TDest : class
            => queryableProvider
                .Query<TEntity>()
                .EitherOrSelf(entitySpec, x => x.Where(entitySpec))
                .ProjectTo<TDest>()
                .EitherOrSelf(destSpec, x => x.Where(destSpec))
                .OrderBy(queryableOrder)
                .ToPagedEnumerable(paging);

        public static TKey Create<TKey, TDto, TEntity>(this IUnitOfWork uow, TDto dto)
            where TEntity : class, IHasId<TKey>
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            var entity = Mapper.Map<TEntity>(dto);
            uow.Add(entity);
            uow.Commit();
            return entity.Id;
        }

        public static void Update<TKey, TEntity, TDto>(this IUnitOfWork uow, TKey id,
            TDto dto)
            where TEntity : class, IHasId<TKey>
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            var entity = uow.Find<TEntity>(id);
            Mapper.Map(dto, entity);
            uow.Commit();
        }

        public static TDest Map<TDest>(this object obj) => Mapper.Map<TDest>(obj);
    }
}