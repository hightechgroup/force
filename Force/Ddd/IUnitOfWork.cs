using System;

namespace Force.Ddd
{
    public interface IUnitOfWork : IDisposable
    {
        void Add<TEntity>(TEntity entity)
            where TEntity : class, IHasId;

        void Remove<TEntity>(TEntity entity)
            where TEntity : class, IHasId;

           
        void Commit();

        void Rollback();
    }
}