using System;
using System.Linq;
using System.Threading.Tasks;

namespace Force.Cqrs.Delegates
{
    public delegate Task<T> FirstOrDefaultAsyncDelegate<T>(IQueryable<T> queryable, Func<T, bool> predicate);
}