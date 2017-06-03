using System.Linq;

namespace Force.Ddd
{
    public interface IQueryableOrder<T>
    {
        IOrderedQueryable<T> Order(IQueryable<T> queryable);
    }
}