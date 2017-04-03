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
            where TKey : IEquatable<TKey>
            where TProjection : class, IHasId<TKey>
            where TEntity : class, IHasId<TKey>
            => query
               .EitherProjectTo<TEntity, TProjection>(configurationProvider)
               .ById(id);

        public static IQueryable<TProjection> ApplyProjectApplyAgain<TEntity, TProjection>(
            this IQueryable<TEntity> queryable, object spec, IConfigurationProvider configurationProvider = null)
            where TEntity : class
            where TProjection : class
            => queryable
                .MaybeWhere(spec)
                .MaybeOrderBy(spec)
                .EitherProjectTo<TEntity, TProjection>(configurationProvider)
                .MaybeWhere(spec)
                .MaybeOrderBy(spec);

        public static IQueryable<TProjection> ApplyProjectApplyAgainWithoutOrderBy<TEntity, TProjection>(
            this IQueryable<TEntity> queryable, object spec, IConfigurationProvider configurationProvider = null)
            where TEntity : class
            where TProjection : class
            => queryable
                .MaybeWhere(spec)
                .ProjectTo<TProjection>(configurationProvider)
                .MaybeWhere(spec);

        public static IQueryable<TProjection> EitherProjectTo<TEntity, TProjection>(this IQueryable<TEntity> queryable,
            IConfigurationProvider configurationProvider = null)
            where TEntity : class
            where TProjection : class
            => queryable
                .Either(configurationProvider != null,
                    x => x.ProjectTo<TProjection>(configurationProvider),
                    x => x.ProjectTo<TProjection>());


        public static PagedResponse<TProjection> Paged<TEntity, TProjection>(this IQueryable<TEntity> query, IPaging spec,
            IConfigurationProvider configurationProvider = null)
            where TEntity : class, IHasId
            where TProjection : class, IHasId => query
                .MaybeWhere(spec)
                .EitherProjectTo<TEntity, TProjection>(configurationProvider)
                .MaybeWhere(spec)
                .MaybeOrderBy(spec)
                .OrderByIdIfNotOrdered()
                .ToPagedResponse(spec);

        public static PagedResponse<TProjection> Paged<TEntity, TProjection>(this IQueryable<TEntity> query,
            IPaging paging, IQueryableOrder<TProjection> queryableOrder, IQueryableFilter<TEntity> entitySpec = null,
            IQueryableFilter<TProjection> projectionSpec = null, IConfigurationProvider configurationProvider = null)
            where TEntity : class, IHasId where TProjection : class
            => query
                .EitherOrSelf(entitySpec, x => x.Where(entitySpec))
                .EitherProjectTo<TEntity, TProjection>(configurationProvider)
                .EitherOrSelf(projectionSpec, x => x.Where(projectionSpec))
                .OrderBy(queryableOrder)
                .ToPagedResponse(paging);

        public static TKey Create<TKey, TDto, TEntity>(this IUnitOfWork uow, TDto dto, IMapper mapper = null)
            where TEntity : class, IHasId<TKey>
            where TKey : IEquatable<TKey>
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
            where TKey : IEquatable<TKey>
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