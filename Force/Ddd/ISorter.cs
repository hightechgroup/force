using System.Linq;

namespace Force.Ddd
{
    public interface ISorter<T>
    {
        IOrderedQueryable<T> Sort(IQueryable<T> queryable);
    }
}