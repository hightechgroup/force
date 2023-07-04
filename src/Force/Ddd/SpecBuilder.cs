using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Force.Expressions;
using Force.Linq;
using Force.Reflection;

namespace Force.Ddd
{
    public static class SpecBuilder<TSubject>
    {
        public static Spec<TSubject> Build<TPredicate>(TPredicate predicate, ComposeKind composeKind = ComposeKind.And)
            => SpecBuilder<TSubject, TPredicate>.Build(predicate, composeKind);
        
        public static Spec<TSubject> BuildSearch<TPredicate>(TPredicate predicate, ComposeKind composeKind = ComposeKind.Or)
            => SpecBuilder<TSubject, TPredicate>.BuildSearch(predicate, composeKind);
    }
    
    public static class SpecBuilder<TSubject, TPredicate>
    {
        // ReSharper disable once StaticMemberInGenericType
        private static readonly List<PropertyInfo> SubjectPropertiesToFilter
            = Type<TSubject>
                .PublicProperties
                .Where(x => Type<TPredicate>
                    .PublicProperties
                    .Keys
                    .Contains(x.Key))
                .Select(x => x.Value)
                .ToList();
        
        public static Spec<TSubject> Build(TPredicate predicate, ComposeKind composeKind = ComposeKind.And)
        {
            var publicProperties = Type<TPredicate>.PublicProperties;
            
            var props = SubjectPropertiesToFilter
                .Select(x => new PropertyInfoAndValue
                {
                    Property = x,
                    Value = publicProperties[x.Name].GetValue(predicate)
                });

            return GetSpec(props, composeKind);
        }

        public static Spec<TSubject> BuildSearch(TPredicate predicate, ComposeKind composeKind = ComposeKind.Or)
        {
            var publicProperties = Type<TPredicate>.PublicProperties;

            var props = SubjectPropertiesToFilter
                .Select(x => new PropertyInfoAndValue
                {
                    Property = x,
                    Value = publicProperties["Search"].GetValue(predicate)
                });

            return GetSpec(props, composeKind);
        }

        private static Spec<TSubject> GetSpec(IEnumerable<PropertyInfoAndValue> props,
            ComposeKind composeKind)
        {
            var parameter = Expression.Parameter(typeof(TSubject));

            var expressions = props
                .Where(x => x.Value != null)
                .Select(x => BuildExpression(parameter, x))
                .Where(x => x != null)
                .ToList();

            if (!expressions.Any())
            {
                return new Spec<TSubject>(x => true);
            }

            var expr = composeKind == ComposeKind.And
                ? expressions.Aggregate((c, n) => c.And(n))
                : expressions.Aggregate((c, n) => c.Or(n));

            return new Spec<TSubject>(expr);
        }

        private static Expression<Func<TSubject, bool>> BuildExpression(ParameterExpression parameter, 
            PropertyInfoAndValue x)
        {
            throw new NotImplementedException();
            // var property = Expression.Property(parameter, x.Property);
            // var val = (x.Value as string)?.ToLower() ?? x.Value;
            // Expression value = Expression.Constant(val);
            //
            // var convention = FilterConventions.Instance.GetConvention(property.Type, x.Value.GetType());
            // if (convention == null)
            // {
            //     return null;
            // }
            //
            // var body = convention.BuildFilterBody(property, value);
            // return Expression.Lambda<Func<TSubject, bool>>(body, parameter);
        }
    }

    internal class PropertyInfoAndValue
    {
        public PropertyInfo Property { get; set; }
        
        public object Value { get; set; }
    }
}