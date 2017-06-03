using System;
using System.Linq;

namespace Force.Ddd.Pagination
{
    public class IdPaging<TEntity, TKey> : Paging<TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class, IHasId<TKey>
    {
        public IdPaging(int page, int take)
            : base(page, take)
        { }

        public IdPaging()
        { }

        public override IOrderedQueryable<TEntity> OrderBy(IQueryable<TEntity> queryable) => queryable.OrderBy(x => x.Id);
    }

    public class IdPaging<TEntity> : IdPaging<TEntity, int>
        where TEntity : class, IHasId<int>
    {
        public IdPaging(int page, int take)
            : base(page, take)
        { }

        public IdPaging()
        { }
    }
}
