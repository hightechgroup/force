using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Force;
using Force.Cqrs;
using Force.Ddd;

namespace Demo.WebApp.Infrastructure.Decorators
{
    public class ValidatorDecorator<TIn, TOut>: HandlerDecoratorBase<TIn, TOut>
    {
        private readonly IEnumerable<IValidator<TIn>> _validators;

        public ValidatorDecorator(IHandler<TIn, TOut> decorated, IEnumerable<IValidator<TIn>> validators) 
            : base(decorated)
        {
            _validators = validators;
        }

        public override TOut Handle(TIn input)
        {
            throw new NotImplementedException();
            //var res = _validators.Validate(input);
//            if (!res.IsValid())
//            {
//                if (typeof(TOut) == typeof(IEnumerable<ValidationResult>))
//                {
//                    return (TOut) res;
//                }
//
//                var message = res
//                    .Select(x => x.ErrorMessage)
//                    .Aggregate((c, n) => $"{c}{Environment.NewLine}{n}");
//                throw new ValidationException(message);
//            }
//
//            return Decorated.Handle(input);
        }
    }
}