using System.Collections.Generic;
using System.Linq;
using Force;
using Force.Ddd;
using Microsoft.EntityFrameworkCore;

namespace Demo.WebApp.Infrastructure
{
    public class QueryableFactory<T>
        where T : class    
    {
        private readonly DbContext _dbContext;
        private readonly IEnumerable<IPermissionFilter<T>> _permissionFilters;

        public QueryableFactory(DbContext dbContext, IEnumerable<IPermissionFilter<T>> permissionFilters)
        {
            _dbContext = dbContext;
            _permissionFilters = permissionFilters;
        }
        
        private IQueryable<T> WithFilters
            => _permissionFilters
                .Aggregate((IQueryable<T>)_dbContext.Set<T>(), 
                    (current, permissionFilter) => permissionFilter.GetPermitted(current));

        public IQueryable<T> ForQuery() 
            => WithFilters.AsNoTracking();

        public IQueryable<T> ForCommand()
            => WithFilters;
    }
}