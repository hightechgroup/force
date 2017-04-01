using System;

namespace Force.Ddd
{
    public interface IHasId
    {
        object Id { get; }
    }

    public interface IHasId<out TKey> : IHasId
        where TKey: IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        new TKey Id { get; }
    }
}
