using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Force.Cqrs;

namespace Force.Extensions
{
    public static class FunctionalExtensions
    {
        public static Func<TSource, TResult> Compose<TSource, TIntermediate, TResult>(
            this Func<TSource, TIntermediate> func1, Func<TIntermediate, TResult> func2)
            => x => func2(func1(x));

        public static TResult PipeTo<TSource, TResult>(
            this TSource source, Func<TSource, TResult> func)
            => func(source);

        public static T PipeToOrSelf<T>(this T obj, Func<T, bool> condition, Func<T, T> action)
            => condition(obj) ? action(obj) : obj;               
        
        public static T PipeToOrSelf<T>(this T obj, Func<T, T> func)
            => PipeToOrSelf(obj, x => x != null, func);
        
        public static TOutput Either<TInput, TOutput>(this TInput o, Func<TInput, TOutput> ifTrue,
            Func<TInput, TOutput> ifFalse)
            => o.Either(x => x != null, ifTrue, ifFalse);

        public static TOutput Either<TInput, TOutput>(this TInput o, Func<TInput, bool> condition,
            Func<TInput, TOutput> ifTrue, Func<TInput, TOutput> ifFalse)
            => condition(o) ? ifTrue(o) : ifFalse(o);

        public static TOutput Either<TInput, TOutput>(this TInput o, bool condition,
            Func<TInput, TOutput> ifTrue, Func<TInput, TOutput> ifFalse)
            => condition ? ifTrue(o) : ifFalse(o);

        public static T Do<T>(this T obj, Func<T, bool> condition, Action<T> action)
        {
            if (condition(obj))
            {
                action(obj);
            }

            return obj;
        }

        public static T Do<T>(this T obj, Action<T> action)
            => Do(obj, x => x != null, action);
        
        #region Cqrs

        public static TResult PipeTo<TSource, TResult>(
            this TSource source, IQuery<TSource, TResult> query)
            => query.Ask(source);

        public static Task<TResult> PipeTo<TSource, TResult>(
            this TSource source, IQuery<TSource, Task<TResult>> query)
            => query.Ask(source);

        public static TResult PipeTo<TSource, TResult>(
            this TSource source, IHandler<TSource, TResult> query)
            => query.Handle(source);

        public static TSource PipeTo<TSource>(
            this TSource source, IHandler<TSource> query)
        {
            query.Handle(source);
            return source;
        }

        public static TOutput Ask<TInput, TOutput>(this IQuery<TInput, TOutput> query)
            where TInput : new()
            => query.Ask(new TInput());

        public static Func<TIn, TOut> ToFunc<TIn, TOut>(this IQuery<TIn, TOut> query)
            => query.Ask;

        public static Func<TIn, TOut> ToFunc<TIn, TOut>(this IHandler<TIn, TOut> handler)
            => handler.Handle;

        #endregion
    }
}