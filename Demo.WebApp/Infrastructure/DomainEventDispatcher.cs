using Force;
using Force.Ddd;

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