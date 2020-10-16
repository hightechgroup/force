using System.Collections.Generic;
using System.Threading.Tasks;

namespace Force.Cqrs
{
    public class FilterQueryAsync<T> 
        : FilterQueryBase<T>
            , IQuery<Task<IEnumerable<T>>>
    {
    }
}