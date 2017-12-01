using System;

namespace Force.Ddd
{
    public abstract class IdentityColumnEntity<TKey>: IHasId<TKey>
        where TKey: IEquatable<TKey>
    {
        object IHasId.Id => Id;

        public TKey Id { get; protected set; }
    }
}