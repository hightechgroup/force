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
        public static TProjection ProjectById<TKey, TEntity, TProjection>(
            this IQueryable<TEntity> query, TKey id, IConfigurationProvider configurationProvider = null)
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
            where TProjection : class, IHasId<TKey>
            where TEntity : class, IHasId<TKey>
            => query
                .ProjectTo<TProjection>(configurationProvider)
                .ById(id);

        public static IQueryable<TDest> ApplyProjectApplyAgain<TSource, TDest>(
            this IQueryable<TSource> queryable, object spec, IConfigurationProvider configurationProvider = null)
            where TSource : class
            where TDest : class
            => queryable
                .MaybeWhere(spec)
                .MaybeOrderBy(spec)
                .ProjectTo<TDest>(configurationProvider)
                .MaybeWhere(spec)
                .MaybeOrderBy(spec);

        public static IQueryable<TDest> ApplyProjectApplyAgainWithoutOrderBy<TSource, TDest>(
            this IQueryable<TSource> queryable, object spec, IConfigurationProvider configurationProvider = null)
            where TSource : class
            where TDest : class
            => queryable
                .MaybeWhere(spec)
                .ProjectTo<TDest>(configurationProvider)
                .MaybeWhere(spec);

        public static PagedResponse<TDest> Paged<TEntity, TDest>(this IQueryable<TEntity> query, IPaging spec,
            IConfigurationProvider configurationProvider = null)
            where TEntity : class, IHasId
            where TDest : class, IHasId => query
                .MaybeWhere(spec)
                .ProjectTo<TDest>(configurationProvider)
                .MaybeWhere(spec)
                .MaybeOrderBy(spec)
                .OrderByIdIfNotOrdered()
                .ToPagedResponse(spec);

        public static PagedResponse<TDest> Paged<TEntity, TDest>(this IQueryable<TEntity> query,
            IPaging paging, IQueryableOrder<TDest> queryableOrder, IQueryableFilter<TEntity> entitySpec = null,
            IQueryableFilter<TDest> destSpec = null, IConfigurationProvider configurationProvider = null)
            where TEntity : class, IHasId where TDest : class
            => query
                .EitherOrSelf(entitySpec, x => x.Where(entitySpec))
                .ProjectTo<TDest>(configurationProvider)
                .EitherOrSelf(destSpec, x => x.Where(destSpec))
                .OrderBy(queryableOrder)
                .ToPagedResponse(paging);

        public static TKey Create<TKey, TDto, TEntity>(this IUnitOfWork uow, TDto dto, IMapper mapper = null)
            where TEntity : class, IHasId<TKey>
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            var mapperInstance = mapper ?? Mapper.Instance;

            var entity = mapperInstance.Map<TEntity>(dto);
            uow.Add(entity);
            uow.Commit();

            return entity.Id;
        }

        public static void Update<TKey, TEntity, TDto>(this IUnitOfWork uow, TKey id,
            TDto dto, IMapper mapper = null)
            where TEntity : class, IHasId<TKey>
            where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            var mapperInstance = mapper ?? Mapper.Instance;

            var entity = uow.Find<TEntity>(id);
            mapperInstance.Map(dto, entity);
            uow.Commit();
        }

        public static TDest Map<TDest>(this object obj, IMapper mapper = null) =>
            mapper
                .EitherOrSelf(x => x == null, _ => Mapper.Instance)
                .Map<TDest>(obj);
    }
}