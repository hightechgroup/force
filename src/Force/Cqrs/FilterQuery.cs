using System.Collections.Generic;

namespace Force.Cqrs
{
    public class FilterQuery<T> 
        : FilterQueryBase<T>
        , IQuery<IEnumerable<T>>

    {
    }
}