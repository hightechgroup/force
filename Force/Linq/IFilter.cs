using System.Linq;
using Force.Ddd;

namespace Force.Linq
{
    public interface IFilter<T>    
    {
        Spec<T> Spec { get; }
    }
    
    public static class FilterExtensions
    {
        public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, IFilter<T> filter)
            => queryable.Where(filter.Spec);
    }
}