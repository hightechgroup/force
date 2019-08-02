using System.Linq;
using Force.Ddd;

namespace Force.Linq
{
    public interface IFilter<T>    
    {
        Spec<T> Spec { get; }
    }
}