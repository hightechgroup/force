using System;
using Force.Ddd.DomainEvents;

namespace Force.Tests.Ddd
{
    public class DomainEvent : IDomainEvent
    {
        public DateTime Happened { get; }
    }
}