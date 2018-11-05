using Force;

namespace Demo.WebApp.Infrastructure
{
    public abstract class HandlerDecoratorBase<T>: IHandler<T>
    {
        protected readonly IHandler<T> Decorated;

        protected HandlerDecoratorBase(IHandler<T> decorated)
        {
            Decorated = decorated;
        }
        
        public abstract void Handle(T input);
    }
}