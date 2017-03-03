using JetBrains.Annotations;

namespace Force.Common
{
    [PublicAPI]
    public interface IMapper
    {
        TReturn Map<TReturn>(object src);

        TReturn Map<TReturn>(object src, TReturn dest);
    }
}