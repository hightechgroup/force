using Force;
using Microsoft.Extensions.Logging;

namespace Demo.WebApp.Infrastructure
{
    public class LoggerHandlerDecorator<T>: HandlerDecoratorBase<T>
    {
        private readonly ILogger _logger;

        public LoggerHandlerDecorator(IHandler<T> decorated, ILogger logger)
            : base(decorated)
        {            
            _logger = logger;
        }

        public override void Handle(T input)
        {
            _logger.LogInformation("Before");
            Decorated.Handle(input);
            _logger.LogInformation("After");
        }
    }
}