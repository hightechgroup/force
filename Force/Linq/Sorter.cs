using System;
using System.Linq;
using System.Linq.Expressions;

namespace Force.Ddd
{
    public class Sorter<T>: ISorter<T>
    {
        private readonly string _propertyName;
        private readonly LambdaExpression _expression;

        public Sorter(LambdaExpression expression)
        {
            _expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        public Sorter(string propertyName)
        {
            _propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
        }
        
        public static implicit operator LambdaExpression(Sorter<T> sorter)
            => sorter._expression;

        public IOrderedQueryable<T> Sort(IQueryable<T> queryable)
            => _propertyName != null
                   ? queryable.OrderBy(_propertyName)
                   : (IOrderedQueryable<T>)((dynamic) queryable).OrderBy(_expression);
    }

    public static class SorterExtensions
    {
        public static IOrderedQueryable<T> Sort<T>(this IQueryable<T> queryable, ISorter<T> sorter)
            => sorter.Sort(queryable);
    }
}