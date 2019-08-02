using System;
using System.Linq;
using Force.Extensions;

namespace Force.Linq
{
    public class AutoFilter<T>
        where T : class
    {
        private readonly object _predicate;
        private readonly string _orderBy;

        public AutoFilter(object predicate, string orderBy = null)
        {
            _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
            _orderBy = orderBy;
        }

        private IQueryable<T> DoFilter<TFilter>(IQueryable<T> queryable, TFilter predicate)
            => queryable.FilterAndSort(predicate);
        
        public IQueryable<T> Filter(IQueryable<T> queryable)
            => DoFilter(queryable, (dynamic)_predicate);

        public IOrderedQueryable<T> Order(IQueryable<T> queryable)
            => !string.IsNullOrEmpty(_orderBy)
                   ? ConventionsExtensions.OrderBy(queryable, _orderBy)
                   : queryable.OrderByFirstProperty();
    }
}