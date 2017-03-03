using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CostEffectiveCode.Ddd;
using CostEffectiveCode.Ddd.Entities;

namespace CostEffectiveCode.Tests.Stubs
{
    public class InMemoryLinqProvider : ILinqProvider
    {
        private readonly Dictionary<Type, IQueryable> _queyables;

        public InMemoryLinqProvider(params IEnumerable[] enumerals)
        {
            _queyables = enumerals.ToDictionary(
                x =>
                {
                    var e = x.GetEnumerator();
                    e.MoveNext();
                    return e.Current.GetType();
                }, x => x.AsQueryable());
        }

        public IQueryable<TEntity> Query<TEntity>() where TEntity : class, IHasId
            => Query(typeof(TEntity)).Cast<TEntity>();

        public IQueryable Query(Type t) => _queyables[t];
    }
}
