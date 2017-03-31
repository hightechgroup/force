using System;
using System.Linq;
using System.Linq.Expressions;
using Force.Extensions;

namespace Force.Ddd
{
    public static class SpecificationExtenions
    {
        public static Specification<T> AsSpec<T>(this Expression<Func<T, bool>> expr)
            where T : class, IHasId
            => new Specification<T>(expr);
    }

    public sealed class Specification<T> : IQueryableFilter<T>
        where T: class, IHasId
    {
        public Expression<Func<T, bool>> Expression { get; }

        public Specification(Expression<Func<T, bool>> expression)
        {
            Expression = expression;
            if (expression == null) throw new ArgumentNullException(nameof(expression));
        }

        public static explicit operator Expression<Func<T, bool>>(Specification<T> spec)
            => spec.Expression;

        public static bool operator false(Specification<T> spec)
        {
            return false;
        }

        public static bool operator true(Specification<T> spec)
        {
            return false;
        }

        public static Specification<T> operator &(Specification<T> spec1, Specification<T> spec2)
            => new Specification<T>(spec1.Expression.And(spec2.Expression));

        public static Specification<T> operator |(Specification<T> spec1, Specification<T> spec2)
            => new Specification<T>(spec1.Expression.Or(spec2.Expression));

        public static Specification<T> operator !(Specification<T> spec)
            => new Specification<T>(spec.Expression.Not());

        public IQueryable<T> Apply(IQueryable<T> query)
            => query.Where(Expression);

        public bool IsSatisfiedBy(T obj) => Expression.AsFunc()(obj);
    }
}