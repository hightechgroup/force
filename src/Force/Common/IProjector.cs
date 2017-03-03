using System.Linq;
using JetBrains.Annotations;

namespace Force.Common
{
    [PublicAPI]
    public interface IProjector
    {
        IQueryable<TReturn> Project<TReturn>(IQueryable queryable);
    }
}
