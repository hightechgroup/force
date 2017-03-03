using System.Collections.Generic;

namespace Force.Ddd.Pagination
{
    public interface IPaging
    {
        int Page { get; }

        int Take { get; }

    }

    public interface IPaging<TEntity, TSortKey> : IPaging
        where TEntity : class
    {

        IEnumerable<Sorting<TEntity, TSortKey>> OrderBy { get; }
    }
}