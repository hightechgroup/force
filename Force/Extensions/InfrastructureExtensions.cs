using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Force.Cqrs;
using Force.Ddd;
using Force.Infrastructure;


namespace Force.Extensions
{
    public static class InfrastructureExtensions
    {
        #region Expressions

        private static readonly ConcurrentDictionary<string, object> Cache
            = new ConcurrentDictionary<string, object>();

        public static Func<TIn, TOut> AsFunc<TIn, TOut>(this Expression<Func<TIn, TOut>> expr)
            //@see http://sergeyteplyakov.blogspot.ru/2015/06/lazy-trick-with-concurrentdictionary.html
            => (Func<TIn, TOut>)((Lazy<object>)Cache
                .GetOrAdd(expr.Body.ToString(), id => new Lazy<object>(expr.Compile))).Value;

        public static bool Is<T>(this T entity, Expression<Func<T, bool>> expr)
            => AsFunc(expr).Invoke(entity);

        
        #endregion

        #region Infrastructure

        public static Func<TA, TR> Memoize<TA, TR>(this Func<TA, TR> f)
        {
            var cache = new SynchronizedConcurrentDictionary<TA, TR>();

            return key => cache.GetOrAdd(key, f);
        }        

        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> me, IDictionary<TKey, TValue> merge)
        {
            foreach (var item in merge)
            {
                me[item.Key] = item.Value;
            }
        }

        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PublicPropertyDictionary 
            = new ConcurrentDictionary<Type, PropertyInfo[]>();

        public static PropertyInfo[] GetPublicProperties(this Type type)
            => PublicPropertyDictionary.GetOrAdd(type, x => x
                .GetTypeInfo()
                .GetProperties()
                .ToArray());
        
        #endregion
    }
}
