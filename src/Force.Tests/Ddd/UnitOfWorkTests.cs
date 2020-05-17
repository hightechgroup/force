using Xunit;

namespace Force.Tests.Ddd
{
    public class UnitOfWorkTests
    {
        [Fact]
        public void Commit()
        {
            // TODO: add MOQ
            var uow = new UnitOfWork(new DomainEventDispatcher());
            uow.Commit();
        }
    }
}