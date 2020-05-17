using System.Linq;
using Force.Ddd;

namespace Force.Linq
{
    public interface IFilter<T>
    {
        IQueryable<T> Filter(IQueryable<T> queryable);
    }
}