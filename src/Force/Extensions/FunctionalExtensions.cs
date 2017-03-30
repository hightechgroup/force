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


        public static T PipeToIfNotNull<T>(this T source, Func<object, T> evaluator)
            where T : class
            => source == null
            ? null
            : PipeTo(source, x => evaluator(x));

        public static TOutput Either<TInput, TOutput>(this TInput o, Func<TInput, TOutput> ifTrue,
            Func<TInput, TOutput> ifFalse)
            => o.Either(x => x != null, ifTrue, ifFalse);

        public static TOutput Either<TInput, TOutput>(this TInput o, Func<TInput, bool> condition,
            Func<TInput, TOutput> ifTrue, Func<TInput, TOutput> ifFalse)
            => condition(o) ? ifTrue(o) : ifFalse(o);


        public static TInput EitherOrSelf<TInput>(this TInput o,
            Func<TInput, bool> condition,
            Func<TInput, TInput> ifTrue)
            where TInput : class
            => condition(o) ? ifTrue(o) : o;

        public static TInput EitherOrSelf<TInput>(this TInput o,
            object obj,
            Func<TInput, TInput> ifTrue)
            where TInput : class
            => obj != null ? ifTrue(o) : o;

        public static T Do<T>(this T obj, Action<T> action)
        {
            if (obj != null)
            {
                action(obj);
            }

            return obj;
        }

        public static bool Satisfy<T>(this T obj, Func<T, bool> spec)
        {
            return spec(obj);
        }

        public static bool SatisfyExpresion<T>(this T obj, Expression<Func<T, bool>> spec)
        {
            return spec.AsFunc()(obj);
        }

        public static bool IsSatisfiedBy<T>(this Func<T, bool> spec, T obj)
        {
            return spec(obj);
        }

        public static bool IsSatisfiedBy<T>(this Expression<Func<T, bool>> spec, T obj)
        {
            return spec.AsFunc()(obj);
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