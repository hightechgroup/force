using System;
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


        public static T PipeTofNotNull<T>(this T source, Func<object, T> evaluator)
            where T : class
            => source == null
            ? null
            : PipeTo(source, x => evaluator(x));

        public static TOutput Either<TInput, TOutput>(this TInput o
            , Func<TInput, bool> condition
            , Func<TInput, TOutput> ifTrue
            , Func<TInput, TOutput> ifFalse)
            where TInput : class
            => condition(o) ? ifTrue(o) : ifFalse(o);

        public static T Do<T>(T obj, Action<T> action)
        {
            action(obj);
            return obj;
        }

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
            => x => query.Ask(x);

        public static Func<TIn, TOut> ToFunc<TIn, TOut>(this IHandler<TIn, TOut> handler)
            => x => handler.Handle(x);

        #endregion
    }
}