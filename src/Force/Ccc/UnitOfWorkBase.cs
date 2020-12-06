using System.Collections.Generic;
using Force.Cqrs;
using Force.Ddd;
using Force.Ddd.DomainEvents;

namespace Force.Ccc
{
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        private readonly IHandler<IEnumerable<IDomainEvent>> _domainEventDispatcher;

        protected UnitOfWorkBase(IHandler<IEnumerable<IDomainEvent>> domainEventDispatcher)
        {
            _domainEventDispatcher = domainEventDispatcher;
        }

        public abstract void Dispose();

        public abstract void Add<TEntity>(TEntity entity)
            where TEntity : class, IHasId;

        public abstract void Remove<TEntity>(TEntity entity)
            where TEntity : class, IHasId;

        public abstract TEntity Find<TEntity>(params object[] id);

        public void Commit()
        {
            var events = GetDomainEvents();
            _domainEventDispatcher.Handle(events);
            DoCommit();
        }

        public abstract IUnitOfWorkTransaction BeginTransaction();

        public abstract void Update<TEntity>(TEntity entity)
            where TEntity : class, IHasId;

        protected abstract void DoCommit();

        protected abstract IEnumerable<IDomainEvent> GetDomainEvents();
    }
}