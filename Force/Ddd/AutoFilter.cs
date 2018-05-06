using System.Linq;
using Force.Extensions;

namespace Force.Ddd
{
    public class AutoFilter<T> : IQueryableFilter<T>, IQueryableOrder<T>
        where T : class, IHasId
    {
        private readonly object _predicate;
        private readonly string _orderBy;

        public AutoFilter(object predicate, string orderBy)
        {
            _predicate = predicate;
            _orderBy = orderBy;
        }

        public IQueryable<T> Filter(IQueryable<T> queryable)
            => queryable.AutoFilter(_predicate);

        public IOrderedQueryable<T> Order(IQueryable<T> queryable)
            => !string.IsNullOrEmpty(_orderBy)
                   ? queryable.AutoSort(_orderBy)
                   : queryable.OrderByIdIfNotOrdered();
    }
}