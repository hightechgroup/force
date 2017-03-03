using System;
using System.Linq;
using Force.Ddd.Entities;
using JetBrains.Annotations;

namespace Force.Ddd
{
    [PublicAPI]
    public interface ILinqProvider

    {
        IQueryable<TEntity> Query<TEntity>()
            where TEntity : class, IHasId;


        IQueryable Query(Type t);
    }
}