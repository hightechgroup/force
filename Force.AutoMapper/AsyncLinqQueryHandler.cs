using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Force.Cqrs;
using Force.Extensions;
using Force.Linq;

namespace Force.AutoMapper
{
    public class AsyncLinqQueryHandler<TEntity, TQuery, TProjection>
        : IQueryHandler<TQuery, Task<IEnumerable<TProjection>>>
        where TQuery : IQuery<Task<IEnumerable<TProjection>>>, IFilter<TProjection>, ISorter<TProjection>
        where TEntity : class
    {
        private readonly IQueryable<TEntity> _entities;

        public AsyncLinqQueryHandler(IQueryable<TEntity> entities)
        {
            _entities = entities;
        }

        public async Task<IEnumerable<TProjection>> Handle(TQuery query)
            => await _entities
                .TryFilter(query)
                .ProjectTo<TProjection>()
                .FilterAndSort(query)
                .TryPaginateAsync(query);
    }
}