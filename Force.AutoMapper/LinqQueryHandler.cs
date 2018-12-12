using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Force.Cqrs;
using Force.Ddd;
using Force.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Force.AutoMapper
{
    public class LinqQueryHandler<TQuery, TEntity, TProjection>
        : IQueryHandler<TQuery, Task<IEnumerable<TProjection>>>
        where TQuery : IQuery<Task<IEnumerable<TProjection>>>, IFilter<TProjection>, ISorter<TProjection>
        where TEntity : class
    {
        private readonly IQueryable<TEntity> _entities;

        public LinqQueryHandler(IQueryable<TEntity> entities)
        {
            _entities = entities;
        }

        public async Task<IEnumerable<TProjection>> Handle(TQuery query)
            => await _entities
                .TryFilter(query)
                .ProjectTo<TProjection>()
                .FilterAndSort(query)
                .TryPaginate(query);
    }
}