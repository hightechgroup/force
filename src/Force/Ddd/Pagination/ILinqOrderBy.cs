using System.Linq;

namespace Force.Ddd.Pagination
{
    public interface ILinqOrderBy<T>
    {
        IOrderedQueryable<T> Apply(IQueryable<T> queryable);
    }
}