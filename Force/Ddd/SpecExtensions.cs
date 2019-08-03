using System;
using System.Linq;
using System.Linq.Expressions;
using Force.Expressions;
using Force.Extensions;
using Force.Infrastructure;
using Force.Linq;

namespace Force.Ddd
{
    public static class SpecExtensions
    {
        public static Spec<T> ToSpec<T>(this Expression<Func<T, bool>> expr)
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