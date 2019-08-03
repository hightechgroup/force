using System;
using System.Collections.Generic;
using Force.Cqrs;
using Force.Ddd.DomainEvents;

namespace Force.Ddd
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

        public abstract void Update<TEntity>(TEntity entity) 
            where TEntity : class, IHasId;

        public abstract void Remove<TEntity>(TEntity entity)
            where TEntity : class, IHasId;

        public abstract TEntity Find<TEntity>(params object[] id);

        protected abstract void DoCommit();
        
        protected abstract IEnumerable<IDomainEvent> GetDomainEvents();

        public void Commit()
        {
            var events = GetDomainEvents();
            _domainEventDispatcher.Handle(events);
            DoCommit();
        }

        public abstract void Rollback();
    }
}