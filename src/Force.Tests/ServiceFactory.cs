using System;
using Microsoft.Extensions.DependencyInjection;

namespace Force.Tests
{
    public class ServiceFactory : IServiceFactory
    {
        private IServiceProvider _sp;
        
        public ServiceFactory(IServiceProvider serviceProvider)
        {
            _sp = serviceProvider;
        }
        
        public T GetService<T>()
        {
            return _sp.GetService<T>();
        }

        public object GetService(Type serviceType)
        {
            return _sp.GetService(serviceType);
        }
    }
}