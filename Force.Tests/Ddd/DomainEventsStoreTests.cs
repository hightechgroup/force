using System;
using System.Collections;
using Force.Ddd.DomainEvents;
using Xunit;

namespace Force.Tests.Ddd
{
    public class DomainEventsStoreTests
    {
        static readonly DomainEvent De = new DomainEvent();
        
        [Fact]
        public void Raise()
        {
            var des = new DomainEventStore();
            des.Raise(De);
            
            AssertEnumerator(des.GetEnumerator());
            AssertEnumerator(((IEnumerable)des).GetEnumerator());
        }

        private void AssertEnumerator(IEnumerator enumerator)
        {
            Assert.True(enumerator.MoveNext());
            Assert.Equal(De, enumerator.Current);
            Assert.False(enumerator.MoveNext());
        }
    }
}