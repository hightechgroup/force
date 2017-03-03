using System;
using System.Collections.Generic;
using Force.Ddd.Entities;

namespace Force.Ddd.Pagination
{
    public class IdPaging<TEntity, TKey>: Paging<TEntity, TKey>
        where TKey: IComparable, IComparable<TKey>, IEquatable<TKey>
        where TEntity : class, IHasId<TKey>
    {
        public IdPaging(int page, int take)
            : base(page, take, new Sorting<TEntity, TKey>(x => x.Id, SortOrder.Desc))
        {
        }

        public IdPaging()
        {
        }

        protected override IEnumerable<Sorting<TEntity, TKey>> BuildDefaultSorting()
        {
            yield return new Sorting<TEntity, TKey>(x => x.Id, SortOrder.Desc);
        }
    }

    public class IdPaging<TEntity>: IdPaging<TEntity, int>
        where TEntity : class, IHasId<int>
    {
        public IdPaging(int page, int take)
            : base(page, take)
        {
        }

        public IdPaging()
        {
        }
    }
}
