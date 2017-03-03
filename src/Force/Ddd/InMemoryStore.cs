using System;
using System.Collections.Concurrent;
using System.Linq;
using Force.Ddd.Entities;

namespace Force.Ddd
{
    public class InMemoryStore : ILinqProvider, IUnitOfWork
    {
        private ConcurrentDictionary<Type, ConcurrentBag<IHasId>> _store = new ConcurrentDictionary<Type, ConcurrentBag<IHasId>>();

        private ConcurrentBag<IHasId> GetBag(Type t) => _store.GetOrAdd(t, _ => new ConcurrentBag<IHasId>());

        private ConcurrentBag<IHasId> GetBag<T>() => GetBag(typeof(T));

        public IQueryable<TEntity> Query<TEntity>() where TEntity : class, IHasId
            => GetBag<TEntity>().Cast<TEntity>().AsQueryable();

        public IQueryable Query(Type t)
            => GetBag(t).AsQueryable();

        public void Dispose()
        {
            _store = new ConcurrentDictionary<Type, ConcurrentBag<IHasId>>();
        }

        public void Add<TEntity>(TEntity entity) where TEntity : class, IHasId
        {
            GetBag<TEntity>().Add(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IHasId
        {
            var bag = GetBag<TEntity>();
            if (bag.Contains(entity))
            {
                _store[typeof(TEntity)] = new ConcurrentBag<IHasId>(bag.Except(new[] {entity}));
            }
        }

        public TEntity Find<TEntity>(object id) where TEntity : class, IHasId
            => (TEntity)GetBag<TEntity>().AsQueryable().FirstOrDefault(x => x.Id.ToString() == id.ToString());

        public IHasId Find(Type entityType, object id)
            => GetBag(entityType).FirstOrDefault(x => x.Id.ToString() == id.ToString());

        public void Commit()
        {
        }
    }
}