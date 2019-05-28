using Force.Cqrs;

namespace Force.Pagination
{
    public interface IPagedQueryHandler<in TIn, TOut>: IQueryHandler<TIn, PagedResponse<TOut>> 
        where TIn : IQuery<PagedResponse<TOut>>
    {        
    }
}