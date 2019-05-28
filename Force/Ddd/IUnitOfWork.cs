using System;

namespace Force.Ddd
{
    public interface IUnitOfWork : IDisposable
    {
        void Add<TEntity>(TEntity entity)
            where TEntity : class, IHasId;

        void Update<TEntity>(TEntity entity)
            where TEntity : class, IHasId;
        
        void Remove<TEntity>(TEntity entity)
            where TEntity : class, IHasId;

        TEntity Find<TEntity>(params object[] id);
            
        void Commit();

        void Rollback();
    }
}