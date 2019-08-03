using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Force.Infrastructure;
using Force.Linq;

namespace Force.Ddd
{
    public static class SpecBuilder<TSubject>
    {
        public static Spec<TSubject> TryBuild<TPredicate>(TPredicate predicate, ComposeKind composeKind = ComposeKind.And)
            => SpecBuilder<TSubject, TPredicate>.TryBuild(predicate, composeKind);
    }
    
    public static class SpecBuilder<TSubject, TPredicate>
    {
        // ReSharper disable once StaticMemberInGenericType
        private static readonly List<PropertyInfo> SubjectPropertiesToFilter;
        
        static SpecBuilder()
        {
            SubjectPropertiesToFilter = Type<TSubject>
                .PublicProperties
                .Where(x => Type<TPredicate>
                    .PublicProperties
                    .Keys
                    .Contains(x.Key))
                .Select(x => x.Value)
                .ToList();           
        }

        
        public static Spec<TSubject> TryBuild(TPredicate predicate, ComposeKind composeKind = ComposeKind.And)
        {
            if (FilterConventions.Instance == null)
            {
                throw new InvalidOperationException("You must call FilterConventions.Initialize first");
            }
            
            var parameter = Expression.Parameter(typeof(TSubject));

            var props = SubjectPropertiesToFilter
                .Select(x => new PropertyInfoAndValue
                {
                    Property = x,
                    Value = Type<TPredicate>
                        .PublicProperties[x.Name]
                        .GetValue(predicate)
                })
                .Where(x => x.Value != null)
                .Select(x => BuildExpression(parameter, x))
                .ToList();

            if (!props.Any())
            {
                return null;
            }

            var expr = composeKind == ComposeKind.And
                ? props.Aggregate((c, n) => c.And(n))
                : props.Aggregate((c, n) => c.Or(n));

            return new Spec<TSubject>(expr);
        }

        private static Expression<Func<TSubject, bool>> BuildExpression(ParameterExpression parameter
            , PropertyInfoAndValue x)
        {
            var property = Expression.Property(parameter, (PropertyInfo) x.Property);
            Expression value = Expression.Constant(x.Value);

            value = Expression.Convert(value, property.Type);
            var body = FilterConventions.Instance[property.Type](property, value);

            return Expression.Lambda<Func<TSubject, bool>>(body, parameter);
        }
    }

    internal class PropertyInfoAndValue
    {
        public PropertyInfo Property { get; set; }
        
        public object Value { get; set; }
    }
}