using System.Linq;

namespace Force.Ddd
{
    public interface IQueryableSpecification<T>
        where T: class 
    {
        IQueryable<T> Apply(IQueryable<T> query);
    }
}
