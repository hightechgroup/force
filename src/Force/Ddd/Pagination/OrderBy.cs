using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Force.Ddd.Pagination
{
    public enum SortOrder
    {
        [PublicAPI] Asc = 1,
        [PublicAPI] Desc = 2
    }

    [PublicAPI]
    public class OrderBy<TEntity, TKey>
        where TEntity: class
    {
        public Expression<Func<TEntity, TKey>> Expression { get; private set; }

        public SortOrder SortOrder { get; private set; }

        public OrderBy(
            Expression<Func<TEntity, TKey>> expression,
            SortOrder sortOrder = SortOrder.Asc)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            Expression = expression;
            SortOrder = sortOrder;
        }
    }
}
