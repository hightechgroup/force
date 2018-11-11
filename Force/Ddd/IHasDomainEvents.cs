using System.Collections;
using System.Collections.Generic;

namespace Force.Ddd
{
    public interface IHasDomainEvents
    {
        IEnumerable GetDomainEvents();
    }

    public interface IHasDomainEvents<out T>: IHasDomainEvents
    {
        new IEnumerable<T> GetDomainEvents();
    }

    public static class HasDomainEventsExtensions
    {
        public static void RaiseDomainEvent<T>(this IHasDomainEvents hasDomainEvent, 
            ICollection<T> domainEvents, T domainEvent)
        {
            domainEvents.Add(domainEvent);
        }
    }
}