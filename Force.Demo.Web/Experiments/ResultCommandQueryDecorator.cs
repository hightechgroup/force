using Force.Cqrs;
using Force.Ddd;

namespace Force.Demo.Web.Experiments
{
    public abstract class ResultCommandQueryDecorator<TSource, TDestination>
        : IQuery<TSource, Result<TDestination>>  
        , IHandler<TSource, Result<TDestination>>  
    
    {
        protected readonly IHandler<TSource, Result<TDestination>> Handler;
        
        protected readonly IQuery<TSource, Result<TDestination>> Query;
        
        public ResultCommandQueryDecorator(IQuery<TSource, Result<TDestination>> query)
        {
            Query = query;
        }
        
        public ResultCommandQueryDecorator(IHandler<TSource, Result<TDestination>> handler)
        {
            Handler = handler;
        }

        protected abstract Result<TDestination> Decorate(TSource value);

        public Result<TDestination> Ask(TSource value)
            => Decorate(value);

        public Result<TDestination> Handle(TSource command)
            => Decorate(command);
    }
}