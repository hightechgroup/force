namespace Force.Components.Cqrs
{
    public interface IHandlerQueryFactory
    {
        IHandler<TIn, TOut> Handler<TIn, TOut>();

        IHandler<TIn> Handler<TIn>();

        IQuery<TIn, TOut> Query<TIn, TOut>();

        IQuery<TIn> Query<TIn>();
    }

    public static class ApiControllerExtensions
    {
        public static TOut Handle<TIn, TOut>(this IHandlerQueryFactory factory, TIn command)
            => factory.Handler<TIn, TOut>().Handle(command);

        public static TOut Ask<TIn, TOut>(this IHandlerQueryFactory factory, TIn spec)
            => factory.Query<TIn, TOut>().Ask(spec);
    }
}