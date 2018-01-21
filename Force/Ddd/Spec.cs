using System;
using System.Linq;
using System.Linq.Expressions;
using Force.Extensions;
using Force.Infrastructure;

namespace Force.Ddd
{
    public class Spec<T> : IQueryableFilter<T>
        where T: class, IHasId
    {
        public static bool operator false(Spec<T> spec) => false;
        
        public static bool operator true(Spec<T> spec) => false;
        
        public static Spec<T> operator &(Spec<T> spec1, Spec<T> spec2)
            => new Spec<T>(spec1.Expression.And(spec2.Expression));

        public static Spec<T> operator |(Spec<T> spec1, Spec<T> spec2)
            => new Spec<T>(spec1.Expression.Or(spec2.Expression));

        public static Spec<T> operator !(Spec<T> spec)
            => new Spec<T>(spec.Expression.Not());
        
        public Expression<Func<T, bool>> Expression { get; }

        public Spec(Expression<Func<T, bool>> expression)
        {
            Expression = expression;
            if (expression == null) throw new ArgumentNullException(nameof(expression));
        }

        public static implicit operator Expression<Func<T, bool>>(Spec<T> spec)
            => spec.Expression;

        public IQueryable<T> Filter(IQueryable<T> query)
            => query.Where(Expression);

        public bool IsSatisfiedBy(T obj) => Expression.AsFunc()(obj);
    }
    
    public static class SpecExtenions
    {
        public static Spec<T> AsSpec<T>(this Expression<Func<T, bool>> expr)
            where T : class, IHasId
            => new Spec<T>(expr);
        
        public static bool Satisfy<T>(this T obj, Func<T, bool> spec)
        {
            return spec(obj);
        }

        public static bool SatisfyExpresion<T>(this T obj, Expression<Func<T, bool>> spec)
        {
            return spec.AsFunc()(obj);
        }

        public static bool IsSatisfiedBy<T>(this Func<T, bool> spec, T obj)
        {
            return spec(obj);
        }

        public static bool IsSatisfiedBy<T>(this Expression<Func<T, bool>> spec, T obj)
        {
            return spec.AsFunc()(obj);
        }  
    }    
}