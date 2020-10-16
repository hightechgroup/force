using System.Threading.Tasks;
using Force.Linq.Pagination;

namespace Force.Cqrs
{
    public class PagedFilterQueryAsync<T> 
        : PagedFilterQueryBase<T>, IQuery<Task<PagedEnumerable<T>>>
    {
        
    }
}