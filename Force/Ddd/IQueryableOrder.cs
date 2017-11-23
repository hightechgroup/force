using System.Linq;

namespace Force.Ddd
{
    public interface IQueryableOrder<T>
    {
        IOrderedQueryable<T> OrderBy(IQueryable<T> queryable);
    }
}