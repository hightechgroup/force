using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Force.Expressions;
using Force.Linq;
using Force.Linq.Conventions;
using Force.Linq.Conventions.Filter;
using Force.Linq.Conventions.Search;
using Force.Reflection;

namespace Force.Ddd
{
    public static class SpecBuilder<TSubject>
    {
        public static Spec<TSubject> Build<TPredicate>(TPredicate predicate, ComposeKind composeKind = ComposeKind.And)
            => SpecBuilder<TSubject, TPredicate>.Build(predicate, composeKind);

        public static Spec<TSubject> BuildSearch<TPredicate>(TPredicate predicate)
            => SpecBuilder<TSubject, TPredicate>.BuildSearch(predicate);

        public static Spec<TSubject> BuildSearchBy<TPredicate>(TPredicate predicate)
            => SpecBuilder<TSubject, TPredicate>.BuildSearchBy(predicate);
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
            var parameter = Expression.Parameter(typeof(TSubject));
            var publicProperties = Type<TPredicate>.PublicProperties;

            var props = SubjectPropertiesToFilter
                .Select(x => new PropertyInfoAndValue
                {
                    Property = x,
                    Value = publicProperties[x.Name].GetValue(predicate)
                })
                .Where(x => x.Value != null)
                .Select(x => BuildExpression(parameter, x, FilterConventions.Instance))
                .Where(x => x != null)
                .ToList();

            return !props.Any()
                ? new Spec<TSubject>(x => true)
                : new Spec<TSubject>(composeKind == ComposeKind.And
                    ? props.Aggregate((c, n) => c.And(n))
                    : props.Aggregate((c, n) => c.Or(n)));
        }

        public static Spec<TSubject> BuildSearchBy(TPredicate predicate)
        {
            var parameter = Expression.Parameter(typeof(TSubject));
            var predicateType = predicate.GetType();

            var spec = new Spec<TSubject>(_ => true);

            var searchValue = predicateType.GetProperty("Search")?.GetValue(predicate) as string;
            var searchByValue = predicateType.GetProperty("SearchBy")?.GetValue(predicate) as string;

            var expression = SubjectPropertiesToFilter
                .Where(x => x.Name.ToUpper() == searchByValue?.ToUpper())
                .Select(x => new PropertyInfoAndValue
                {
                    Property = x,
                    Value = searchValue?.ToLower()
                })
                .Where(x => x.Property.PropertyType == typeof(string)
                            && x.Property.GetCustomAttribute<SearchByAttribute>() != null && x.Value != null)
                .Select(x => BuildExpression(parameter, x, SearchConvensions.Instance))
                .FirstOrDefault();

            return expression == null ? spec : new Spec<TSubject>(expression);
        }

        public static Spec<TSubject> BuildSearch(TPredicate predicate, ComposeKind composeKind = ComposeKind.Or)
        {
            var parameter = Expression.Parameter(typeof(TSubject));

            var spec = new Spec<TSubject>(x => true);

            var search = predicate.GetType().GetProperty("Search")?.GetValue(predicate) as string;
            var expressions = SubjectPropertiesToFilter
                .Select(x => new PropertyInfoAndValue
                {
                    Property = x,
                    Value = search?.ToLower()
                })
                .Where(x => x.Property.PropertyType == typeof(string)
                            && x.Property.GetCustomAttribute<SearchByAttribute>() != null && x.Value != null)
                .Select(x => BuildExpression(parameter, x, SearchConvensions.Instance))
                .ToArray();

            return !expressions.Any()
                ? spec
                : new Spec<TSubject>(composeKind == ComposeKind.And
                    ? expressions.Aggregate((c, n) => c.And(n))
                    : expressions.Aggregate((c, n) => c.Or(n)));
        }

        private static Expression<Func<TSubject, bool>> BuildExpression<T>(ParameterExpression parameter,
            PropertyInfoAndValue x, ConventionsBase<T> conventions) where T : IConvention
        {
            var property = Expression.Property(parameter, x.Property);
            var val = (x.Value as string)?.ToLower() ?? x.Value;
            Expression value = Expression.Constant(val);

            var convention = conventions.GetConvention(property.Type, x.Value.GetType());
            if (convention == null)
            {
                return null;
            }

            var body = convention.BuildBody(property, value);
            return Expression.Lambda<Func<TSubject, bool>>(body, parameter);
        }
    }

    internal class PropertyInfoAndValue
    {
        public PropertyInfo Property { get; set; }

        public object Value { get; set; }
    }
}