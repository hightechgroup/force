using Force;
using Force.Ddd;

namespace Demo.WebApp.Infrastructure.Decorators
{
    public class UowDecorator<TIn, TOut>: HandlerDecoratorBase<TIn, TOut>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UowDecorator(IHandler<TIn, TOut> decorated, IUnitOfWork unitOfWork) : base(decorated)
        {
            _unitOfWork = unitOfWork;
        }

        public override TOut Handle(TIn input)
        {
            var output = Decorated.Handle(input);
            _unitOfWork.Commit();;
            return output;
        }
    }
}