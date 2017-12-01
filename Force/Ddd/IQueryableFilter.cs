using System.Linq;

namespace Force.Ddd
{
    public interface IQueryableFilter<T>
        where T: class 
    {
        IQueryable<T> Filter(IQueryable<T> query);
    }
}
