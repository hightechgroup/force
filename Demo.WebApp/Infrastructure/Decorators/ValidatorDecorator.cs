using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Force;
using Force.Ddd;

namespace Demo.WebApp.Infrastructure.Decorators
{
    public class ValidatorDecorator<T>: IHandler<T, IEnumerable<ValidationResult>>
    {
        private readonly IHandler<T> _decorated;
        private readonly IEnumerable<IValidator<T>> _validators;

        public ValidatorDecorator(IHandler<T> decorated, 
            IEnumerable<IValidator<T>> validators)
        {
            _decorated = decorated;
            _validators = validators;
        }

        public IEnumerable<ValidationResult> Handle(T input)
        {
            var res = _validators.Validate(input);
            if (res.IsValid())
            {
                _decorated.Handle(input);
            }

            return res;
        }
    }
}