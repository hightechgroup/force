using Force;

namespace Demo.WebApp.Infrastructure.Decorators
{   
    public abstract class HandlerDecoratorBase<TIn, TOut>: IHandler<TIn, TOut>
    {
        protected readonly IHandler<TIn, TOut> Decorated;

        protected HandlerDecoratorBase(IHandler<TIn, TOut> decorated)
        {
            Decorated = decorated;
        }
        
        public abstract TOut Handle(TIn input);
    }
}