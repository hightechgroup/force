using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Force.Cqrs;
using Force.Ddd;
using Force.Extensions;
using Force.Linq;
using Microsoft.EntityFrameworkCore;

namespace Force.AutoMapper
{
    public class LinqQueryHandler<TEntity, TQuery, TProjection>
        : IQueryHandler<TQuery, IEnumerable<TProjection>>
        where TEntity : class
        where TProjection: IHasId
        where TQuery : IQuery<IEnumerable<TProjection>>, IFilter<TProjection>, ISorter<TProjection>
    {
        private readonly IQueryable<TEntity> _entities;

        public LinqQueryHandler(IQueryable<TEntity> entities)
        {
            _entities = entities;
        }

        public IEnumerable<TProjection> Handle(TQuery query)
            => _entities
                //.TryFilter(query)
                .ProjectTo<TProjection>()
                .FilterAndSort(query)
                .TryPaginate(query);
    }
}