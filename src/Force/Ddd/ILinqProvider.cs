using System;
using System.Linq;
using Force.Ddd.Entities;

namespace Force.Ddd
{
    public interface ILinqProvider

    {
        IQueryable<TEntity> Query<TEntity>()
            where TEntity : class, IHasId;


        IQueryable Query(Type t);
    }
}