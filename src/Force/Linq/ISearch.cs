using System.Linq;

namespace Force.Linq
{
    public interface ISearch<T>
    {
        IQueryable<T> SearchItem(IQueryable<T> queryable);
    }
}