using System;
using JetBrains.Annotations;

namespace Force.Common
{
    [PublicAPI]
    public interface IScope<out T> : IDisposable
    {
        T Instance { get; }
    }
}
