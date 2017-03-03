using System;
using System.Linq;
using Force.Common;
using Force.Ddd;
using Force.Ddd.Entities;
using Force.Extensions;
using JetBrains.Annotations;

namespace Force.Cqrs.Queries
{
    public class GetByIdQuery<TKey, TEntity, TProjection> : IQuery<TKey, TProjection>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        where TEntity : class, IHasId<TKey>
    {
        protected readonly ILinqProvider LinqProvider;
        protected readonly IProjector Projector;
        protected readonly IMapper Mapper;

        public GetByIdQuery(ILinqProvider linqProvider, IProjector projector, [NotNull] IMapper mapper)
        {
            if (linqProvider == null) throw new ArgumentNullException(nameof(linqProvider));
            if (projector == null) throw new ArgumentNullException(nameof(projector));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));

            LinqProvider = linqProvider;
            Projector = projector;
            Mapper = mapper;
        }

        public virtual TProjection Ask(TKey specification) => typeof(TProjection).GetMapType() != MapType.Func
                ? Query(specification).Project<TProjection>(Projector).SingleOrDefault()
                : Mapper.Map<TProjection>(Query(specification).SingleOrDefault());

        private IQueryable<TEntity> Query(TKey specification) => LinqProvider
                .Query<TEntity>()
                .Where(x => specification.Equals(x.Id));
    }
}
