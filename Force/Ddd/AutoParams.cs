using System;
using System.Linq;
using System.Linq.Expressions;

namespace Force.Ddd
{
    public class AutoParams<T>
        : IQueryableFilter<T>
        , IQueryableOrder<T>
    
        where T : class, IHasId
    {
        public IQueryable<T> Filter(IQueryable<T> query)
        {
            throw new NotImplementedException();
        }

        public IOrderedQueryable<T> Order(IQueryable<T> queryable)
        {
            throw new NotImplementedException();
        }
    }
}