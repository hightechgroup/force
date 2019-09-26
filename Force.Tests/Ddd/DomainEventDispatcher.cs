using System.Collections.Generic;
using Force.Cqrs;
using Force.Ddd.DomainEvents;

namespace Force.Tests.Ddd
{
    public class DomainEventDispatcher: IHandler<IEnumerable<IDomainEvent>>
    {
        public void Handle(IEnumerable<IDomainEvent> input)
        {
        }
    }
}