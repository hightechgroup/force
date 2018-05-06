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
        internal static IQueryable<TProjection> ProjectToWithConfigurationOrFallback<TEntity, TProjection>(this IQueryable<TEntity> queryable,
            IConfigurationProvider configurationProvider = null)
            where TEntity : class
            where TProjection : class
            => queryable
                .EitherOr(configurationProvider != null,
                    x => x.ProjectTo<TProjection>(configurationProvider),
                    x => x.ProjectTo<TProjection>());
        
        public static TProjection ProjectById<TKey, TEntity, TProjection>(
            this IQueryable<TEntity> query, TKey id, IConfigurationProvider configurationProvider = null)
            where TKey : IEquatable<TKey>
            where TProjection : class, IHasId<TKey>
            where TEntity : class, IHasId<TKey>
            => query
               .ProjectToWithConfigurationOrFallback<TEntity, TProjection>(configurationProvider)
               .ById(id);


        public static PagedResponse<TProjection> SmartPaging<TEntity, TProjection>(this IQueryable<TEntity> query, 
            ISmartPaging<TEntity, TProjection> smartPaging, IConfigurationProvider configurationProvider = null)
            where TEntity : class, IHasId
            where TProjection : class, IHasId => query
                .PipeToIf(_ => smartPaging.Spec != null, x => x.Where(smartPaging.Spec))
                .ProjectToWithConfigurationOrFallback<TEntity, TProjection>(configurationProvider)
                .PipeToIf(_ => smartPaging.Filter != null, x => x.Where(smartPaging.Filter))
                .PipeToIf(_ => smartPaging.Order != null, x => x.OrderBy(smartPaging.Order))
                .OrderByIdIfNotOrdered()
                .ToPagedResponse(smartPaging);


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

        public static TDest Map<TDest>(this object obj, IMapper mapper = null)
            => (mapper ?? Mapper.Instance).Map<TDest>(obj);
    }
}