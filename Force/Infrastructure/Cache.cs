using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;

namespace Force.Infrastructure
{
    public static class Cache<T>
    {
         private static readonly ConcurrentDictionary<int, T> _cache 
             = new ConcurrentDictionary<int, T>();

        public static T Get(Func<T> func)
        {
            var forClojure = func;
            return _cache.GetOrAdd(func.GetHashCode(), forClojure()); 
        }

        public static T Get(Func<T> func, TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }
    }
}