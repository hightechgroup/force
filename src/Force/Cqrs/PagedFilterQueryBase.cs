using System.ComponentModel.DataAnnotations;
using Force.Linq.Pagination;

namespace Force.Cqrs
{
    public class PagedFilterQueryBase<T> 
        : FilterQueryBase<T>
            , IPaging
    {
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;
        
        [Range(1, int.MaxValue)]
        public int Take { get; set; } = 1;
    }
}