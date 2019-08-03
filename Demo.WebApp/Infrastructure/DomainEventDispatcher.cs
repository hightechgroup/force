using Force;
using Force.Cqrs;
using Force.Ddd;
using Force.Ddd.DomainEvents;

namespace Demo.WebApp.Infrastructure
{
    public class DomainEventDispatcher: IHandler<IDomainEvent>
    {
        public void Handle(IDomainEvent input)
        {
            throw new System.NotImplementedException();
        }
    }
}