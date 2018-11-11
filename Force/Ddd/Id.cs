using System;

namespace Force.Ddd
{
    public class Id<T>: Id<int, T>
        where T : class, IHasId<int>
    {
        public static bool TryParse(int value, Func<int, T> loader, out Id<T> id)
        {
            if (value <= 0)
            {
                id = null;
                return true;
            }
            
            id = new Id<T>(value, loader);
            return false;
        }
        
        public Id(T entity) : base(entity)
        {
        }

        public Id(int value, Func<int, T> loader) : base(value, loader)
        {
            if(value <= 0)
            {
                throw new ArgumentException("Id must have value", nameof(value));
            }
        }
    }
    
    public class Id<TKey, T>
        where T: class, IHasId<TKey>
        where TKey : IEquatable<TKey>
    {        
        public Id(T entity)
        {
            _entity = entity ?? throw new ArgumentNullException(nameof(entity));
            if (entity.IsNew())
            {
                throw new ArgumentException("Id must have value", nameof(entity));
            }
            
            Value = entity.Id;
        }

        public Id(TKey value, Func<TKey, T> loader)
        {
            if(Equals(value, default(TKey)))
            {
                throw new ArgumentException("Id must have value", nameof(value));
            }
            
            Value = value;
            _loader = loader ?? throw new ArgumentNullException(nameof(loader));            
        }

        public static bool TryParse(TKey value, Func<TKey, T> loader, out Id<TKey, T> id)
        {
            if (Equals(value, default(TKey)))
            {
                id = null;
                return true;
            }
            
            id = new Id<TKey, T>(value, loader);
            return false;
        }

        private T _entity;
        
        private readonly Func<TKey, T> _loader;
        
        public TKey Value { get; }

        public T Entity => _entity ?? (_entity = _loader(Value));

        public static implicit operator TKey(Id<TKey, T> id)
            => id.Value;

        public static implicit operator T(Id<TKey, T> id)
            => id.Entity;
    }
}