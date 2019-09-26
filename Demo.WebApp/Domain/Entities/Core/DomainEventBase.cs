using System;
using Force.Ddd.DomainEvents;

namespace Demo.WebApp.Domain.Entities.Core
{
    public abstract class DomainEventBase: IDomainEvent
    {
        public DateTime Happened { get; protected set; } = DateTime.UtcNow;
        
        public DateTime? Handled { get; protected set; }

        public void SetHandled()
        {
            Handled = DateTime.UtcNow;
        }
    }
}