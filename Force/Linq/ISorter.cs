using System.Linq;

namespace Force.Linq
{
    public interface ISorter<T>
    {
        IOrderedQueryable<T> Sort(IQueryable<T> queryable);
    }
}