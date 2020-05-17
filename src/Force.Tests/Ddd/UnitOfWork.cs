using System.Collections.Generic;
using Force.Ccc;
using Force.Cqrs;
using Force.Ddd;
using Force.Ddd.DomainEvents;

namespace Force.Tests.Ddd
{
    public class UnitOfWork: UnitOfWorkBase
    {
        public UnitOfWork(IHandler<IEnumerable<IDomainEvent>> domainEventDispatcher) : base(domainEventDispatcher)
        {
        }

        public override void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public override void Add<TEntity>(TEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public override void Remove<TEntity>(TEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public override void Rollback()
        {
            throw new System.NotImplementedException();
        }

        public override TEntity Find<TEntity>(params object[] id)
        {
            throw new System.NotImplementedException();
        }

        protected override void DoCommit()
        {
        }

        protected override IEnumerable<IDomainEvent> GetDomainEvents()
        {
            return new IDomainEvent[] { };
        }

        public override void Update<TEntity>(TEntity entity)
        {
            throw new System.NotImplementedException();
        }
    }
}