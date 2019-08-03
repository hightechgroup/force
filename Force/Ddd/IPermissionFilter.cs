using System.Linq;

namespace Force.Ddd
{
    public interface IPermissionFilter<T>
    {
        IQueryable<T> GetPermitted(IQueryable<T> queryable);
    }
}