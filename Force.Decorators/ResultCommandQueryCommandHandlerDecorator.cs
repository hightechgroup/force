using System;
using Force.Cqrs;
using Force.Ddd;

namespace Force.Demo.Web.Experiments
{
    public abstract class ResultCommandQueryCommandHandlerDecorator<TSource, TDestination>
        : IQueryHandler<TSource, Result<TDestination>>  
        , ICommandHandler<TSource, Result<TDestination>>  
    
    {
        private readonly Func<TSource, Result<TDestination>> _func;

        protected ResultCommandQueryCommandHandlerDecorator(
            Func<TSource, Result<TDestination>> func)
        {
            _func = func;
        }

        protected ResultCommandQueryCommandHandlerDecorator(
            IQueryHandler<TSource, Result<TDestination>> query)
            : this(query.Handle)
        {
        }
 
        protected ResultCommandQueryCommandHandlerDecorator(
            ICommandHandler<TSource, Result<TDestination>> query)
            : this(query.Handle)
        {
        }
        
        protected abstract Result<TDestination> Decorate(
            Func<TSource, Result<TDestination>> func, TSource value);

        public Result<TDestination> Ask(TSource param)
            => Decorate(_func, param);

        public Result<TDestination> Handle(TSource command)
            => Decorate(_func, command);
    }
}