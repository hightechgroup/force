using System;
using System.ComponentModel.DataAnnotations;
using Force.Ddd;

namespace DemoApp.Domain
{
    public sealed class EntityId<T>
        where T : IEquatable<T>
    {
        private T _id;

        internal EntityId(EntityBase<T> entity)
        {
            _id = entity.Id;
        }
        
        public static implicit operator T(EntityId<T> obj) => obj._id;
        
        public static implicit operator EntityId<T> (T obj) => null;
    }

    public class EntityBase<T> : HasIdBase<T>
        where T : IEquatable<T>
    {
        [Key, Required]
        public override T Id { get; set; }
    }

}