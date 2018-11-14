using System;
using Force;
using Microsoft.Extensions.Logging;

namespace Demo.WebApp.Infrastructure.Decorators
{
    public class LoggerDecorator<TIn, TOut>: HandlerDecoratorBase<TIn, TOut>
    {
        private readonly ILogger _logger;

        private Type _decoratedType;
        
        public LoggerDecorator(IHandler<TIn, TOut> decorated, ILogger logger)
            : base(decorated)
        {            
            _logger = logger;
            _decoratedType = decorated.GetType();
        }

        public override TOut Handle(TIn input)
        {
            var output = Decorated.Handle(input);
            _logger.LogInformation($"{_decoratedType}: {input} => {output}");
            return output;
        }
    }
}