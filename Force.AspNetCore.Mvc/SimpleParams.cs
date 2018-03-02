using System.Linq;
using Force.Ddd;
using Force.Ddd.Pagination;

namespace Force.AspNetCore.Mvc
{
    public class SimpleParams<TEntity> 
        : IdPaging<TEntity>, IQueryableFilter<TEntity>
        where TEntity : class, IHasId<int>
    {
        public IQueryable<TEntity> Filter(IQueryable<TEntity> query) => query;
    }
}