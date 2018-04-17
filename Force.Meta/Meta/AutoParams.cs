using System;
using System.Linq;
using System.Linq.Expressions;
using FastMember;
using Force.Ddd;

namespace Force.Meta
{
    public abstract class AutoParams<T>
        : IQueryableFilter<T>
        , IQueryableOrder<T>
    
        where T : class, IHasId
    {
        public string OrderBy { get; set; }
        
        public ComposeKind ComposeKind { get; set; }
        
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