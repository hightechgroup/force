using Xunit;

namespace Force.Tests.Ddd
{
    public class UnitOfWorkTests
    {
        [Fact]
        public void Commit()
        {
            var uow = new UnitOfWork(new DomainEventDispatcher());
            uow.Commit();
        }
    }
}