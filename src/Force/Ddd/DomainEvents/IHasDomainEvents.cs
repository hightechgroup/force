using System.Collections.Generic;

namespace Force.Ddd.DomainEvents
{
    public interface IHasDomainEvents: IHasDomainEvents<IDomainEvent>
    {
    }

    public interface IHasDomainEvents<out T>
        where T: IDomainEvent
    {
        IEnumerable<T> GetDomainEvents();
    }
}