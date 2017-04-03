using System;
using System.Linq;
using System.Linq.Expressions;
using Force.Extensions;

namespace Force.Ddd
{
    public static class SpecExtenions
    {
        public static Spec<T> AsSpec<T>(this Expression<Func<T, bool>> expr)
            where T : class, IHasId
            => new Spec<T>(expr);
    }

    public sealed class Spec<T> : IQueryableFilter<T>
        where T: class, IHasId
    {
        public Expression<Func<T, bool>> Expression { get; }

        public Spec(Expression<Func<T, bool>> expression)
        {
            Expression = expression;
            if (expression == null) throw new ArgumentNullException(nameof(expression));
        }

        public static explicit operator Expression<Func<T, bool>>(Spec<T> spec)
            => spec.Expression;

        public static bool operator false(Spec<T> spec)
        {
            return false;
        }

        public static bool operator true(Spec<T> spec)
        {
            return false;
        }

        public static Spec<T> operator &(Spec<T> spec1, Spec<T> spec2)
            => new Spec<T>(spec1.Expression.And(spec2.Expression));

        public static Spec<T> operator |(Spec<T> spec1, Spec<T> spec2)
            => new Spec<T>(spec1.Expression.Or(spec2.Expression));

        public static Spec<T> operator !(Spec<T> spec)
            => new Spec<T>(spec.Expression.Not());

        public IQueryable<T> Apply(IQueryable<T> query)
            => query.Where(Expression);

        public bool IsSatisfiedBy(T obj) => Expression.AsFunc()(obj);
    }
}