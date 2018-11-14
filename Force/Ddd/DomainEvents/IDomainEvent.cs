using System;

namespace Force.Ddd
{
    public interface IDomainEvent
    {
        DateTime Happened { get; }
    }
}