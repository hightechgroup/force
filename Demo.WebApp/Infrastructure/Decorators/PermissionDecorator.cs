using System.Security;
using Force;
using Force.Cqrs;

namespace Demo.WebApp.Infrastructure.Decorators
{
    public abstract class PermissionDecorator<TIn, TOut>: HandlerDecoratorBase<TIn, TOut>
    {
        protected PermissionDecorator(IHandler<TIn, TOut> decorated) : base(decorated)
        {
        }

        protected abstract bool CheckPermissions(TIn input);

        public override TOut Handle(TIn input)
        {
            if (!CheckPermissions(input))
            {
                throw new SecurityException();
            }

            return Decorated.Handle(input);
        }
    }
}