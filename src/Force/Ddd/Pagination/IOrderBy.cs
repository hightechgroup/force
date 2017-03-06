using System.Collections.Generic;

namespace Force.Ddd.Pagination
{
    public interface IOrderBy<TEntity, TSortKey> where TEntity : class
    {
        IEnumerable<OrderBy<TEntity, TSortKey>> OrderBy { get; }
    }
}