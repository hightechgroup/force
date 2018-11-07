using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Demo.WebApp.Infrastructure
{
    public class QueryableFactory
    {
        private readonly DbContext _dbContext;

        public QueryableFactory(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> ForQuery<T>() 
            where T : class
            => _dbContext.Set<T>().AsNoTracking();
        
        public IQueryable<T> ForCommand<T>() 
            where T : class
            => _dbContext.Set<T>();        
    }
}