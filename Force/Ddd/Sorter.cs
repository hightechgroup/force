using System;
using System.Linq;
using System.Linq.Expressions;

namespace Force.Ddd
{
    public class Sorter<T>
    {
        private readonly LambdaExpression _expression;

        public Sorter(LambdaExpression expression)
        {
            _expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }
        
        public static implicit operator LambdaExpression(Sorter<T> sorter)
            => sorter._expression;

        public IOrderedQueryable<T> Order(IQueryable<T> queryable)
            => (IOrderedQueryable<T>)((dynamic) queryable).OrderBy(_expression);
    }
}