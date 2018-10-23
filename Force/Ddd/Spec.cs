using System;
using System.Linq;
using System.Linq.Expressions;
using Force.Extensions;
using Force.Infrastructure;

namespace Force.Ddd
{
    public class Spec<T>        
    {
        public static bool operator false(Spec<T> spec) => false;
        
        public static bool operator true(Spec<T> spec) => false;
        
        public static Spec<T> operator &(Spec<T> spec1, Spec<T> spec2)
            => new Spec<T>(spec1._expression.And(spec2._expression));

        public static Spec<T> operator |(Spec<T> spec1, Spec<T> spec2)
            => new Spec<T>(spec1._expression.Or(spec2._expression));

        public static Spec<T> operator !(Spec<T> spec)
            => new Spec<T>(spec._expression.Not());
        
        public static implicit operator Expression<Func<T, bool>>(Spec<T> spec)
            => spec._expression;
        
        public static implicit operator Spec<T>(Expression<Func<T, bool>> expression)
            => new Spec<T>(expression);

        private readonly Expression<Func<T, bool>> _expression;

        public Spec(Expression<Func<T, bool>> expression)
        {
            _expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        public bool IsSatisfiedBy(T obj) => _expression.AsFunc()(obj);

        public Spec<TParent> From<TParent>(Expression<Func<TParent, T>> mapFrom)
            => _expression.From(mapFrom);
    }
    
    public static class SpecExtenions
    {
        public static Spec<T> ToSpec<T>(this Expression<Func<T, bool>> expr)
            where T : class, IHasId
            => new Spec<T>(expr);

        public static Spec<T> ToSpec<T>(this object predicate, ComposeKind composeKind = ComposeKind.And)
        {
            var filterProps = predicate.GetType()
                .GetProperties()
                .Where(x => x.CanRead && x.CanWrite)
                .ToArray();

            var filterPropNames = filterProps
                .Select(x => x.Name)
                .ToArray();

            var modelType = typeof(T);
            var stringType = typeof(string);
            var dateTimeType = typeof(DateTime);
            var dateTimeNullableType = typeof(DateTime?);

            var parameter = Expression.Parameter(modelType);

            var props = FastTypeInfo<T>
                .PublicProperties
                .Where(x => filterPropNames.Contains(x.Name))
                .Select(x => new
                {
                    Property = x,
                    Value = filterProps.Single(y => y.Name == x.Name).GetValue(predicate)
                })
                .Where(x => x.Value != null)
                .Select(x =>
                {
                    var property = Expression.Property(parameter, x.Property);
                    Expression value = Expression.Constant(x.Value);

                    value = Expression.Convert(value, property.Type);
                    var body = Conventions.Filters[property.Type](property, value);
                        
                    return Expression.Lambda<Func<T, bool>>(body, parameter);
                })
                .ToArray();

            if (!props.Any())
            {
                return null;
            }

            return composeKind == ComposeKind.And
                ? props.Aggregate((c, n) => c.And(n))
                : props.Aggregate((c, n) => c.Or(n));
        }
        
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