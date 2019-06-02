using AutoMapper.QueryableExtensions;
using Force.Cqrs;
using Force.Ddd;
using Force.Extensions;
using Force.Linq;
using Force.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Force.AutoMapper
{
    public class PagedLinqQueryHandler<TQuery, TEntity, TProjection>
        : IQueryHandler<TQuery, PagedResponse<TProjection>>
        where TQuery : IQuery<PagedResponse<TProjection>>, IFilter<TProjection>, ISorter<TProjection>, IPaging
        where TEntity : class
    {
        private readonly DbContext _dbContext;

        public PagedLinqQueryHandler(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public PagedResponse<TProjection> Handle(TQuery query)
            => _dbContext
                .Set<TEntity>()
                .TryFilter(query)
                .ProjectTo<TProjection>()
                .FilterAndSort(query)
                .ToPagedResponse(query);
    }
}