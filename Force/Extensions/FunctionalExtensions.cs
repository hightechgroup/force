using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Force.Infrastructure;

namespace Force.Extensions
{
    public static class FunctionalExtensions
    {       
        public static TResult PipeTo<TSource, TResult>(
            this TSource source, Func<TSource, TResult> func)
            => func(source);

        public static TSource PipeToIf<TSource>(
            this TSource source, Func<TSource, bool> predicate, Func<TSource, TSource> func)
            => predicate(source)
                ? func(source)
                : source;
        
        public static Func<TSource, TResult> Compose<TSource, TIntermediate, TResult>(
            this Func<TSource, TIntermediate> func1, Func<TIntermediate, TResult> func2)
            => x => func2(func1(x));

        
        public static TOutput EitherOr<TInput, TOutput>(this TInput o, Func<TInput, TOutput> ifTrue,
            Func<TInput, TOutput> ifFalse)
            => o.EitherOr(x => x != null, ifTrue, ifFalse);

        public static TOutput EitherOr<TInput, TOutput>(this TInput o, Func<TInput, bool> condition,
            Func<TInput, TOutput> ifTrue, Func<TInput, TOutput> ifFalse)
            => condition(o) ? ifTrue(o) : ifFalse(o);

        public static TOutput EitherOr<TInput, TOutput>(this TInput o, bool condition,
            Func<TInput, TOutput> ifTrue, Func<TInput, TOutput> ifFalse)
            => condition ? ifTrue(o) : ifFalse(o);

        
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

        public static void Invoke<T>(this IHandler<T> handler, T input, Action<T, IHandler<T>> decorator)
        {
            decorator(input, handler);
        }
        
        public static TOut Invoke<TIn, TOut>(this IHandler<TIn, TOut> handler, TIn input,
            Func<TIn, IHandler<TIn, TOut>, TOut> decorator)
        {
            return decorator(input, handler);
        }
        
        public static void Invoke<TIn, TOut>(this IHandler<TIn, TOut> handler, TIn input,
            Action<TIn, IHandler<TIn, TOut>> decorator)
        {
            decorator(input, handler);
        }
    }
}