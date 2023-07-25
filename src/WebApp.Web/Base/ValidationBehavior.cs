using FluentValidation;

namespace WebApp.Web.Base;

[UsedImplicitly]
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var errorsDict = new Dictionary<string, List<string>>();
                foreach (var result in validationResults)
                {
                    foreach (var error in result.Errors)
                    {
                        if (errorsDict.ContainsKey(error.PropertyName))
                        {
                            errorsDict[error.PropertyName].Add(error.ErrorMessage);
                        }
                        else
                        {
                            errorsDict.Add(error.PropertyName, new List<string> {error.ErrorMessage});
                        }
                    }
                }

                if (errorsDict.Any())
                {
                    throw new CustomValidationException(errorsDict);
                }
            }
            return await next();
        }
    }