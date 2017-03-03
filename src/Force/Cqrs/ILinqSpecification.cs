using System.Linq;

namespace Force.Cqrs
{
    public interface ILinqSpecification<T>
        where T: class 
    {
        IQueryable<T> Apply(IQueryable<T> query);
    }
}
