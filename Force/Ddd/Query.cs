using System.Linq;

namespace Force.Ddd
{
    public class Query<T>: IFilter<T>
    {
        public Query()
        {            
        }
        
        public Query(Spec<T> spec, Sorter<T> sorter = null)
        {
            Spec = spec;
            Sorter = sorter;
        }

        public virtual Spec<T> Spec { get; }
        
        public virtual Sorter<T> Sorter { get; }               
    }
}