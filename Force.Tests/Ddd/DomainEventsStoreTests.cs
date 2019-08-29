using System.Collections;
using Force.Ddd.DomainEvents;
using Xunit;

namespace Force.Tests.Ddd
{
    public class DomainEventsStoreTests
    {
        [Fact]
        public void Raise()
        {
            var de = new DomainEventStore();
            de.Raise(new DomainEvent());
            de.GetEnumerator();
            ((IEnumerable)de).GetEnumerator();
        }
    }
}