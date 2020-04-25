using System.Linq;

namespace Force.Ccc
{
    public interface IPermissionFilter<T>
    {
        IQueryable<T> GetPermitted(IQueryable<T> queryable);
    }
}