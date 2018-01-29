using System;
using System.Collections.Concurrent;

namespace Force.Infrastructure
{
    public static class Cache<T>
    {
         private static readonly ConcurrentDictionary<int, T> _cache 
             = new ConcurrentDictionary<int, T>();

        public static T Get(Func<T> func)
            => _cache.GetOrAdd(func.GetHashCode(), func());
    }
}