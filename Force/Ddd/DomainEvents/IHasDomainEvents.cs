using System.Collections;
using System.Collections.Generic;

namespace Force.Ddd
{
    public interface IHasDomainEvents: IHasDomainEvents<IDomainEvent>
    {
    }

    public interface IHasDomainEvents<out T>
        where T: IDomainEvent
    {
        IEnumerable<T> GetDomainEvents();
    }

    public static class HasDomainEventsExtensions
    {
        public static void Raise<T>(this IHasDomainEvents hasDomainEvent, 
            ICollection<T> domainEvents, T domainEvent)
        {
            domainEvents.Add(domainEvent);
        }
    }
}