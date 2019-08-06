using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Force.Ddd;
using Force.Extensions;
using Force.Linq;
using Force.Linq.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Force.AutoMapper
{
    public static class Extensions
    {
        public static IEnumerable<T> TryPaginate<T>(this IQueryable<T> queryable, object maybePaging)
            where T: IHasId
        {
            if (maybePaging is IPaging paging)
            {
                var orderedQueryable = (queryable as IOrderedQueryable<T>) ?? queryable.OrderById();
                var list = (orderedQueryable.Paginate(paging)).ToList(); 
                return new PagedEnumerable<T>(list, queryable.Count());
            }

            return queryable.ToList();
        }
        
        public static async Task<IEnumerable<T>> TryPaginateAsync<T>(this IQueryable<T> queryable, object maybePaging)
        {
            throw new NotImplementedException();

//            if (maybePaging is IPaging paging)
//            {
//                var orderedQueryable = (queryable as IOrderedQueryable<T>) ?? queryable.OrderByFirstProperty();
//                var list = await (orderedQueryable.Paginate(paging)).ToListAsync(); 
//                return new PagedEnumerable<T>(list, queryable.Count());
//            }
//
//            return await queryable.ToListAsync();
        }
        
        internal static IQueryable<TProjection> ProjectToWithConfigurationOrFallback<TEntity, TProjection>(this IQueryable<TEntity> queryable,
            IConfigurationProvider configurationProvider = null)
            where TEntity : class
            where TProjection : class
            => queryable
                .EitherOr(configurationProvider != null,
                    x => x.ProjectTo<TProjection>(configurationProvider),
                    x => x.ProjectTo<TProjection>());
        

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
        
        public static TDest Map<TDest>(this object obj, IMapper mapper = null)
            => (mapper ?? Mapper.Instance).Map<TDest>(obj);
    }
}