using System;

namespace Force.Ddd
{
    public interface IHasId
    {
        object Id { get; }
    }

    public interface IHasId<out TKey> : IHasId
        where TKey: IEquatable<TKey>
    {
        new TKey Id { get; }
    }

    public static class HasIdExtensions
    {
        public static bool IsNew<TKey>(this IHasId<TKey> obj)
            where TKey : IEquatable<TKey>
        {
            return obj.Id == null || obj.Id.Equals(default);
        }
    }
}
