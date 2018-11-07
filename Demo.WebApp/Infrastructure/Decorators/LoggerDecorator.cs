using Force;
using Microsoft.Extensions.Logging;

namespace Demo.WebApp.Infrastructure.Decorators
{
    public class LoggerDecorator<TIn, TOut>: HandlerDecoratorBase<TIn, TOut>
    {
        private readonly ILogger _logger;

        public LoggerDecorator(IHandler<TIn, TOut> decorated, ILogger logger)
            : base(decorated)
        {            
            _logger = logger;
        }

        public override TOut Handle(TIn input)
        {
            _logger.LogInformation($"input: {input}");
            var output = Decorated.Handle(input);
            _logger.LogInformation($"output: {output}");

            return output;
        }
    }
}