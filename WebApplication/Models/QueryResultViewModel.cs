using Force.Cqrs;

namespace WebApplication.Models
{
    public class QueryResultViewModel<T>
    {
        public QueryResultViewModel(IQuery<T> query, T result)
        {
            Query = query;
            Result = result;
        }

        public IQuery<T> Query { get; }
        
        public T Result { get; }
    }
}