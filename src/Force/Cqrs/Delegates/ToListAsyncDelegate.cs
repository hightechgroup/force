using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Force.Cqrs.Delegates
{
    public delegate Task<List<T>> ToListAsyncDelegate<T>(IQueryable<T> queryable);
}