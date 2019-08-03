using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Force.Expressions
{
    internal class CompiledExpressions<TIn, TOut>
    {
        private static readonly ConcurrentDictionary<Expression<Func<TIn, TOut>>, Func<TIn, TOut>> Cache
            = new ConcurrentDictionary<Expression<Func<TIn, TOut>>, Func<TIn, TOut>>();

        internal static Func<TIn, TOut> AsFunc(Expression<Func<TIn, TOut>> expr)
            => Cache.GetOrAdd(expr, k => k.Compile());
    }
}