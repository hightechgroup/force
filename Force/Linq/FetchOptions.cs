using System.Linq;

namespace Force.Ddd
{
    public class FetchOptions<T>: IFilter<T>, ISorter<T>
    {
        public FetchOptions()
        {            
        }
        
        public FetchOptions(Spec<T> spec, Sorter<T> sorter)
        {
            Spec = spec;
            Sorter = sorter;
        }

        public Spec<T> Spec { get; }
        
        protected Sorter<T> Sorter { get; }

        public IOrderedQueryable<T> Sort(IQueryable<T> queryable) => Sorter.Sort(queryable);
    }
}