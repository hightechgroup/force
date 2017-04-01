using System.Linq;

namespace Force.Ddd
{
    public interface IQueryableFilter<T>
        where T: class 
    {
        IQueryable<T> Apply(IQueryable<T> query);
    }
}
