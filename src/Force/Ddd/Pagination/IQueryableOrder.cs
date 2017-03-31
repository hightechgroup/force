using System.Linq;

namespace Force.Ddd.Pagination
{
    public interface IQueryableOrder<T>
    {
        IOrderedQueryable<T> Apply(IQueryable<T> queryable);
    }
}