using System.Linq;

namespace Force.Ddd.Pagination
{
    public interface IQueryableOrderBy<T>
    {
        IOrderedQueryable<T> Apply(IQueryable<T> queryable);
    }
}