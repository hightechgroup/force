using Force.Examples.Data;
using Force.Examples.Domain.Features;

namespace Force.Tests.InfrastructureTests
{
    public abstract class TestsWithServiceProviderBase
    {
        private readonly IServiceFactory _serviceProvider;

        protected IServiceFactory GetServiceProvider()
        {
            return _serviceProvider;
        }

        protected TestsWithServiceProviderBase()
        {
            _serviceProvider = new ServiceFactory(Services.BuildServiceProvider(
                sp => new ExampleDbContext(),
                typeof(ProductController).Assembly));
        }
    }
}