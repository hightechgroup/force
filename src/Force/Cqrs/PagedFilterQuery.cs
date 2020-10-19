using Force.Linq.Pagination;

namespace Force.Cqrs
{
    public class PagedFilterQuery<T>
        : PagedFilterQueryBase<T>
        , IQuery<PagedEnumerable<T>>
    {
    }
}