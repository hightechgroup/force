using System.Linq;

namespace Force.Ddd
{
    public class Query<T>
    {
        public Query()
        {            
        }
        
        public Query(Spec<T> spec, Sorter<T> sorter)
        {
            Spec = spec;
            Sorter = sorter;
        }

        public Spec<T> Spec { get; }
        
        public Sorter<T> Sorter { get; }               
    }
}