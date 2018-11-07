using System.Linq;

namespace Force.Ddd
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