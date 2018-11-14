using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Force.Cqrs;
using Force.Ddd;
using Force.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Force.AutoMapper
{
    public class LinqQueryHandler<TQuery, TEntity, TProjection>
        : IQueryHandler<TQuery, IEnumerable<TProjection>>
        where TQuery : IQuery<IEnumerable<TProjection>>, IFilter<TProjection>, ISorter<TProjection>
        where TEntity : class
    {
        private IQueryable<TEntity> _entities;

        public LinqQueryHandler(IQueryable<TEntity> entities)
        {
            _entities = entities;
        }

        public IEnumerable<TProjection> Handle(TQuery query)
            => _entities
                .TryFilter(query)
                .ProjectTo<TProjection>()
                .FilterAndSort(query)
                .ToList();        
    }
}