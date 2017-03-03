using System.Linq;

namespace Force
{
    public interface ILinqOrderBy<T>
    {
        IOrderedQueryable<T> Apply(IQueryable<T> queryable);
    }
}