using Force.Ddd.DomainEvents;
using Xunit;

namespace Force.Tests.Ddd
{
    public class DomainEventTests
    {
        [Fact]
        public void A()
        {
            var de = new DomainEventStore();
            de.Raise(new DomainEvent());
        }
    }
}