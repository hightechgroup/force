using System.Linq;

namespace Force.Ddd.Specifications
{
    public interface ILinqSpecification<T>
        where T: class 
    {
        IQueryable<T> Apply(IQueryable<T> query);
    }
}
