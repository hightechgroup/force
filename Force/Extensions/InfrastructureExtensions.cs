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
    internal class CompiledExpressions<TIn, TOut>
    {
        private static readonly ConcurrentDictionary<Expression<Func<TIn, TOut>>, Func<TIn, TOut>> Cache
            = new ConcurrentDictionary<Expression<Func<TIn, TOut>>, Func<TIn, TOut>>();

        internal static Func<TIn, TOut> AsFunc(Expression<Func<TIn, TOut>> expr)
            => Cache.GetOrAdd(expr, k => k.Compile());
    }
    
    public static class InfrastructureExtensions
    {
        #region Expressions

        private static readonly ConcurrentDictionary<string, object> Cache
            = new ConcurrentDictionary<string, object>();

        public static Func<TIn, TOut> AsFunc<TIn, TOut>(this Expression<Func<TIn, TOut>> expr)
            => CompiledExpressions<TIn, TOut>.AsFunc(expr);

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
        
        #endregion
    }
}
