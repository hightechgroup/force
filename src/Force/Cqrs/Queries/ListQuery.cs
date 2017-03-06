using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Force.Ddd;
using Force.Ddd.Entities;
using Force.Ddd.Specifications;
using Force.Extensions;

namespace Force.Cqrs.Queries
{
    /// <summary>
    /// Query for fetching list entites by specification.
    /// Support ILinqSpecification&lt;TEntity&gt;, Expression&lt;Func&lt;TEntity,bool&gt;&gt;, ExpressionSpecification&lt;TEntity&gt; as specification
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public class ListQuery<TEntity>
        : IQuery<ILinqSpecification<TEntity>, IEnumerable<TEntity>>
        , IQuery<ILinqSpecification<TEntity>, int>

        , IQuery<Expression<Func<TEntity,bool>>, IEnumerable<TEntity>>
        , IQuery<Expression<Func<TEntity,bool>>, int>

        , IQuery<ExpressionSpecification<TEntity>, IEnumerable<TEntity>>
        , IQuery<ExpressionSpecification<TEntity>, int>
        where TEntity: class, IHasId
    {
        protected readonly ILinqProvider LinqProvider;

        public ListQuery(ILinqProvider linqProvider)
        {
            if (linqProvider == null) throw new ArgumentNullException(nameof(linqProvider));
            LinqProvider = linqProvider;
        }

        protected virtual IQueryable<TEntity> Query(object spec)
            => LinqProvider
                .Query<TEntity>()
                .Apply(spec);

        protected virtual IQueryable<TEntity> CountQuery(object spec)
            => LinqProvider.Query<TEntity>().MaybeWhere(spec);

        IEnumerable<TEntity> IQuery<ILinqSpecification<TEntity>, IEnumerable<TEntity>>.Ask(ILinqSpecification<TEntity> spec)
            => Query(spec).ToArray();

        int IQuery<ILinqSpecification<TEntity>, int>.Ask(ILinqSpecification<TEntity> spec)
            => CountQuery(spec).Count();


        public IEnumerable<TEntity> Ask(Expression<Func<TEntity, bool>> spec)
            => Query(spec).ToArray();

        int IQuery<Expression<Func<TEntity, bool>>, int>.Ask(Expression<Func<TEntity, bool>> spec)
            => CountQuery(spec).Count();

        public IEnumerable<TEntity> Ask(ExpressionSpecification<TEntity> spec)
            => Query(spec).ToArray();

        int IQuery<ExpressionSpecification<TEntity>, int>.Ask(ExpressionSpecification<TEntity> spec)
            => CountQuery(spec).Count();
    }
}