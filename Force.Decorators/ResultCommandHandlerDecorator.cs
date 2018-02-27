using Force.Cqrs;
using Force.Ddd;

namespace Force.Demo.Web.Shop.Catalog
{
    public abstract class ResultCommandHandlerDecorator<TSource>: ICommandHandler<TSource, Result>
    {
        private readonly ICommandHandler<TSource, Result> _commandHandler;

        public ResultCommandHandlerDecorator(ICommandHandler<TSource, Result> commandHandler)
        {
            _commandHandler = commandHandler;
        }

        protected abstract Result Decorate(TSource value);

        public Result Handle(TSource value)
            => Decorate(value) && _commandHandler.Handle(value);
    }
}