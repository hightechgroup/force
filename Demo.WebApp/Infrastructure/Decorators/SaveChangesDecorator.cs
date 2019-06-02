using System.Linq;
using Force;
using Force.Ddd;
using Force.Ddd.DomainEvents;
using Microsoft.EntityFrameworkCore;

namespace Demo.WebApp.Infrastructure.Decorators
{
    public class SaveChangesDecorator<TIn, TOut>: IHandler<TIn, TOut>
    {
        private readonly IHandler<TIn, TOut> _decorated;
        private readonly DbContext _dbContext;
        private readonly DomainEventDispatcher _dispatcher;

        public SaveChangesDecorator(IHandler<TIn, TOut> decorated, DbContext dbContext, DomainEventDispatcher dispatcher)
        {
            _decorated = decorated;
            _dbContext = dbContext;
            _dispatcher = dispatcher;
        }

        public TOut Handle(TIn input)
        {
            var res = _decorated.Handle(input);
            _dbContext.ChangeTracker
                .Entries()
                .Where(x => x is IHasDomainEvents)
                .SelectMany(x => ((IHasDomainEvents) x).GetDomainEvents())
                .ToList()
                .ForEach(_dispatcher.Handle);
            
            
            _dbContext.SaveChanges();
            return res;
        }
    }
}