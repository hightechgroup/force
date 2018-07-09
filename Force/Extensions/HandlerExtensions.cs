using System;
using System.Threading.Tasks;

namespace Force.Extensions
{
    public static class HandlerExtensions
    {
        public static TResult PipeTo<TSource, TResult>(
            this TSource source, IHandler<TSource, TResult> queryHandler)
            => queryHandler.Handle(source);

        public static Task<TResult> PipeTo<TSource, TResult>(
            this TSource source, IHandler<TSource, Task<TResult>> queryHandler)
            => queryHandler.Handle(source);

        public static TSource PipeTo<TSource>(
            this TSource source, IHandler<TSource> query)
        {
            query.Handle(source);
            return source;
        }

        public static Func<TIn, TOut> ToFunc<TIn, TOut>(this  IHandler<TIn, TOut> handler)
            => handler.Handle;

    }
}