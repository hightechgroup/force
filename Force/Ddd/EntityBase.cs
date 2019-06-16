using System;
using System.ComponentModel;

namespace Force.Ddd
{
    public abstract class EntityBase : EntityBase<int>
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override EntityBase<int> WithId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must have value", nameof(id));    
            }
            
            return base.WithId(id);
        }
    }
    
    public abstract class EntityBase<TKey>: IHasId<TKey>
        where TKey : IEquatable<TKey>
    {
        object IHasId.Id => Id;
        
        public TKey Id { get; protected set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual EntityBase<TKey> WithId(TKey id)
        {
            if (Object.Equals(id, default(TKey)))
            {
                throw new ArgumentException("Id must have value", nameof(id));    
            }
            
            Id = id;
            return this;
        }
        
        public override bool Equals(object obj)
        {
            var other = obj as EntityBase<TKey>;

            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (GetType() != other.GetType())
            {
                return false;
            }

            if (Object.Equals(Id, default(TKey)) || Object.Equals(other.Id, default(TKey)))
            {
                return false;
            }

            return Object.Equals(Id, other.Id);
        }

        public static bool operator == (EntityBase<TKey> a, EntityBase<TKey>  b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator != (EntityBase<TKey>  a, EntityBase<TKey>  b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().ToString() + Id).GetHashCode();
        }
    }
}