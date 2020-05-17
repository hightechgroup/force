using System;

namespace Force.Ddd.DomainEvents
{
    public interface IDomainEvent
    {
        DateTime Happened { get; }
    }
}