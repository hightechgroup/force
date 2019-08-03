using System;
using Force;
using Force.Cqrs;
using StackExchange.Profiling;

namespace Demo.WebApp.Infrastructure.Decorators
{
    public class ProfilerDecorator<TIn, TOut>: HandlerDecoratorBase<TIn, TOut>
    {
        private readonly Type _decoratedType;
        
        public ProfilerDecorator(IHandler<TIn, TOut> decorated) : base(decorated)
        {
            _decoratedType = decorated.GetType();
        }

        public override TOut Handle(TIn input)
        {
            using (MiniProfiler.Current.Step(_decoratedType.ToString()))
            {
                return Decorated.Handle(input);
            }
        }
    }
}