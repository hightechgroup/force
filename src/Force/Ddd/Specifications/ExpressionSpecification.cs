using System;
using System.Linq.Expressions;
using Force.Extensions;
using JetBrains.Annotations;

namespace Force.Ddd.Specifications
{
    [PublicAPI]
    public class ExpressionSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> Expression { get; private set; }

        private Func<T, bool> Func => Expression.AsFunc();

        public ExpressionSpecification([NotNull] Expression<Func<T, bool>> expression)
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
