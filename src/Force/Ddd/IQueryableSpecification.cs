using System.Linq;

namespace Force.Ddd.Specifications
{
    public interface IQueryableSpecification<T>
        where T: class 
    {
        IQueryable<T> Apply(IQueryable<T> query);
    }
}
