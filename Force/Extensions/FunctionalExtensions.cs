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

        public static T DoIf<T>(this T obj, Func<T, bool> condition, Action<T> action)
        {
            if (condition(obj))
            {
                action(obj);
            }

            return obj;
        }

        public static T DoIfNotNull<T>(this T obj, Action<T> action)
        {
            if (obj != null)
            {
                action(obj);
            }

            return obj;
        }
        
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
    }
}