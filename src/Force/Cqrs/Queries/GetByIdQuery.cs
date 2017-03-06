using System;
using System.Linq;
using Force.Common;
using Force.Ddd;
using Force.Ddd.Entities;
using Force.Extensions;
using Force.Meta;

namespace Force.Cqrs.Queries
{
    /// <summary>
    /// Query for fetching entity by Id
    /// </summary>
    /// <typeparam name="TKey">Id type</typeparam>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public class GetByIdQuery<TKey, TEntity> : IQuery<TKey, TEntity>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        where TEntity : class, IHasId<TKey>
    {
        protected readonly ILinqProvider LinqProvider;

        public GetByIdQuery(ILinqProvider linqProvider)
        {
            LinqProvider = linqProvider;
        }

        /// <summary>
        /// Get entity by Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Entity</returns>
        public virtual TEntity Ask(TKey id) => LinqProvider.Query<TEntity>().SingleOrDefault(x => x.Id.Equals(id));
    }

    /// <summary>
    /// Query for fetching entity by Id
    /// </summary>
    /// <typeparam name="TKey">Id type</typeparam>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TProjection">Projection type. IQueryable&lt;T&gt;.Select(x => new TProjection(){...}).SingleOrDefault()</typeparam>
    public class GetByIdQuery<TKey, TEntity, TProjection>
        : GetByIdQuery<TKey, TEntity>
        , IQuery<TKey, TProjection>
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        where TEntity : class, IHasId<TKey>
    {
        protected readonly IProjector Projector;
        protected readonly IMapper Mapper;

        public GetByIdQuery(ILinqProvider linqProvider, IProjector projector, IMapper mapper) :base(linqProvider)
        {
            if (projector == null) throw new ArgumentNullException(nameof(projector));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));

            Projector = projector;
            Mapper = mapper;
        }

        private IQueryable<TEntity> Query(TKey id) => LinqProvider
                .Query<TEntity>()
                .Where(x => x.Id.Equals(id));

        /// <summary>
        /// Get entity projection by Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Entity projection</returns>
        public new virtual TProjection Ask(TKey id) => typeof(TProjection).GetMapType() != MapType.Func
            ? Query(id).Project<TProjection>(Projector).SingleOrDefault()
            : Mapper.Map<TProjection>(Query(id).SingleOrDefault());
    }
}
