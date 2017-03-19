using System;
using System.Linq.Expressions;
using Force.Extensions;

namespace Force.Ddd.Specifications
{
    public class ExpressionSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> Expression { get; }

        private Func<T, bool> Func => Expression.AsFunc();

        public ExpressionSpecification(Expression<Func<T, bool>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            Expression = expression;
        }

        public bool IsSatisfiedBy(T o)
        {
            return Func(o);
        }
    } 
}
