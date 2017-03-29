using System;
using System.Linq;

namespace Force.Ddd
{
    public interface IQueryableProvider
    {
        IQueryable<TEntity> Query<TEntity>()
            where TEntity : class, IHasId;
    }
}