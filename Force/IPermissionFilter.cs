using System.Linq;

namespace Force
{
    public interface IPermissionFilter<T>
    {
        IQueryable<T> GetPermitted(IQueryable<T> queryable);
    }
}