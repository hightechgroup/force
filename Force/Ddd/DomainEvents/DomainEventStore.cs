using System.Collections;
using System.Collections.Generic;

namespace Force.Ddd.DomainEvents
{
    public class DomainEventStore: IEnumerable<IDomainEvent>
    {
        public List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        public void Raise(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public IEnumerator<IDomainEvent> GetEnumerator()
            => _domainEvents.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _domainEvents.GetEnumerator();
    }
}