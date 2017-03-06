using System.Linq;
using JetBrains.Annotations;

namespace Force.Common
{
    /// <summary>
    /// Projection builder for IQueryable. Used to avoid queryable.Select(x => {Id = x.Id, Name = x.Name, CategoryId = x.Category.Id} boilerplate
    /// </summary>
    [PublicAPI]
    public interface IProjector
    {
        /// <summary>
        /// Map to specific IQueryable&lt;T&gt;
        /// </summary>
        /// <param name="queryable">Source queriable</param>
        /// <typeparam name="TDest">Destination queryable (projection)</typeparam>
        /// <returns>Destinaion queryable</returns>
        IQueryable<TDest> Project<TDest>(IQueryable queryable);
    }
}
