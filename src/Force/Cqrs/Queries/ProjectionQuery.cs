using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Force.Common;
using Force.Ddd;
using Force.Ddd.Entities;
using Force.Ddd.Specifications;
using Force.Extensions;

namespace Force.Cqrs.Queries
{
    /// <summary>
    /// Query for fetching list entity projections by specification.
    /// Support ILinqSpecification&lt;TEntity&gt;, Expression&lt;Func&lt;TEntity,bool&gt;&gt;, ExpressionSpecification&lt;TEntity&gt; as specification
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TProjection">Projection type</typeparam>
    public class ProjectionQuery<TEntity, TProjection>
        : ListQuery<TEntity>
        , IQuery<ILinqSpecification<TProjection>, IEnumerable<TProjection>>
        , IQuery<ILinqSpecification<TProjection>, int>

        , IQuery<Expression<Func<TProjection,bool>>, IEnumerable<TProjection>>
        , IQuery<Expression<Func<TProjection,bool>>, int>

        , IQuery<ExpressionSpecification<TProjection>, IEnumerable<TProjection>>
        , IQuery<ExpressionSpecification<TProjection>, int>

        where TEntity : class, IHasId
        where TProjection : class
    {
        protected readonly IProjector Projector;

        public ProjectionQuery(ILinqProvider linqProvider, IProjector projector) : base(linqProvider)
        {
            if (projector == null) throw new ArgumentNullException(nameof(projector));

            Projector = projector;
        }

        protected virtual IQueryable<TProjection> ProjectQuery(object spec)
            => Query(spec).Project<TProjection>(Projector).Apply(spec);

        IEnumerable<TProjection> IQuery<ILinqSpecification<TProjection>, IEnumerable<TProjection>>.Ask(ILinqSpecification<TProjection> spec)
            => ProjectQuery(spec).ToArray();

        int IQuery<ILinqSpecification<TProjection>,int>.Ask(ILinqSpecification<TProjection> spec)
            => CountQuery(spec).Count();

        IEnumerable<TProjection> IQuery<Expression<Func<TProjection,bool>>, IEnumerable<TProjection>>.Ask(Expression<Func<TProjection, bool>> spec)
            => ProjectQuery(spec).ToArray();

        int IQuery<Expression<Func<TProjection, bool>>, int>.Ask(Expression<Func<TProjection, bool>> spec)
            => CountQuery(spec).Count();

        IEnumerable<TProjection> IQuery<ExpressionSpecification<TProjection>, IEnumerable<TProjection>>.Ask(ExpressionSpecification<TProjection> spec)
            => ProjectQuery(spec).ToArray();

        int IQuery<ExpressionSpecification<TProjection>, int>.Ask(ExpressionSpecification<TProjection> spec)
            => CountQuery(spec).Count();

    }
}
