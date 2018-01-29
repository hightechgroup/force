using Force.Cqrs;
using Force.Ddd;

namespace Force.Demo.Web.Shop.Catalog
{
    public abstract class ResultHandlerDecorator<TSource>: IHandler<TSource, Result>
    {
        private readonly IHandler<TSource, Result> _handler;

        public ResultHandlerDecorator(IHandler<TSource, Result> handler)
        {
            _handler = handler;
        }

        protected abstract Result Decorate(TSource value);

        public Result Handle(TSource value)
            => Decorate(value) && _handler.Handle(value);
    }
}