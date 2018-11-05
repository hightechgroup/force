using System.Collections.Generic;
using Force;
using Force.Ddd;

namespace Demo.WebApp.Infrastructure
{
    public class ValidatorHandlerDecorator<T>: HandlerDecoratorBase<T>
    {
        private readonly IEnumerable<IValidator<T>> _validators;

        public ValidatorHandlerDecorator(IHandler<T> decorated, IEnumerable<IValidator<T>> validators) 
            : base(decorated)
        {
            _validators = validators;
        }

        public override void Handle(T input)
        {
            //_validators.Validate(input);
            Decorated.Handle(input);
        }
    }
}