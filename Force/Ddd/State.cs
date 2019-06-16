using System;

namespace Force.Ddd
{
    public abstract class State<TEntity>
        where TEntity: class 
    {
        protected State(TEntity entity)
        {
            Entity = entity
                     ?? throw new ArgumentNullException(nameof(entity));
        }

        protected TEntity Entity { get; }
    }
}