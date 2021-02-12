using System;

namespace Force
{
    public interface IServiceFactory : IServiceProvider
    {
        T GetService<T>();
    }
}