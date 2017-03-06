using System;
using System.ComponentModel.DataAnnotations;

namespace Force.Ddd.Entities
{
    public abstract class HasIdBase<TKey> : IHasId<TKey>
        where TKey: IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        [Key, Required]
        public virtual TKey Id { get; set; }

        object IHasId.Id => Id;
    }
}
