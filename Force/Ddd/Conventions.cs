using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Force.Infrastructure;

namespace Force.Ddd
{
    public enum ComposeKind
    {
        And, Or
    }
    
    public static class ConventionsExtensions
    {
        public static IQueryable<TSubject> AutoFilter<TSubject, TPredicate>(
            this IQueryable<TSubject> query, TPredicate predicate, ComposeKind composeKind = ComposeKind.And)
        {
            var filtered = Conventions<TSubject>.Filter(query, predicate, composeKind);
            var orderBy = FastTypeInfo<TPredicate>.PublicProperties.FirstOrDefault(x => x.Name == "OrderBy");
            var proprtyName = orderBy?.GetValue(predicate, null) as string;
            
            return proprtyName == null
                ? filtered
                : Conventions<TSubject>.Sort(filtered, proprtyName);
        }

        public static IOrderedQueryable<TSubject> AutoSort<TSubject>(this IQueryable<TSubject> query, string propertyName)
            => Conventions<TSubject>.Sort(query, propertyName);
    }
    
    public static class Conventions<TSubject>
    {
        private static ConcurrentDictionary<string, Expression> _sorters 
            = new ConcurrentDictionary<string, Expression>();

        private static MethodInfo StartsWith = typeof(string)
            .GetMethod("StartsWith", new[] {typeof(string)});
        
        public static IOrderedQueryable<TSubject> Sort(IQueryable<TSubject> query, string propertyName)
        {
            var property = FastTypeInfo<TSubject>
                .PublicProperties
                .FirstOrDefault(x => x.Name == propertyName);

            if (property == null)
            {
                throw new InvalidOperationException($"There is no public property \"{propertyName}\" " +
                                                    $"in type \"{typeof(TSubject)}\"");
            }

            var parameter = Expression.Parameter(typeof(TSubject));
            var body = Expression.Property(parameter, propertyName);

            var lambda = FastTypeInfo<Expression>
                .PublicMethods
                .First(x => x.Name == "Lambda");

            lambda = lambda.MakeGenericMethod(typeof(Func<,>)
                .MakeGenericType(typeof(TSubject), property.PropertyType));

            var expression = lambda.Invoke(null, new object[] {body, new[] {parameter}});

            var orderBy = typeof(Queryable)
                .GetMethods()
                .First(x => x.Name == "OrderBy" && x.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(TSubject), property.PropertyType);

            return (IOrderedQueryable<TSubject>) orderBy.Invoke(query, new object[] {query, expression});
        }

        public static IQueryable<TSubject> Filter<TPredicate>(IQueryable<TSubject> query,
            TPredicate predicate,
            ComposeKind composeKind = ComposeKind.And)
        {
            var filterProps = FastTypeInfo<TPredicate>
                .PublicProperties
                .ToArray();

            var filterPropNames = filterProps
                .Select(x => x.Name)
                .ToArray();

            var modelType = typeof(TSubject);
            var stringType = typeof(string);
            var dateTimeType = typeof(DateTime);
            var dateTimeNullableType = typeof(DateTime?);

            var parameter = Expression.Parameter(modelType);

            var props = FastTypeInfo<TSubject>
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
                    Expression property = Expression.Property(parameter, x.Property);
                    Expression value = Expression.Constant(x.Value);

                    value = Expression.Convert(value, property.Type);
                    var body = property.Type == typeof(string)
                                   ? (Expression)Expression.Call(property, StartsWith, value)
                                   : Expression.Equal(property, value);

                    return Expression.Lambda<Func<TSubject, bool>>(body, parameter);
                })
                .ToArray();

            if (!props.Any())
            {
                return query;
            }

            var expr = composeKind == ComposeKind.And
                ? props.Aggregate((c, n) => c.And(n))
                : props.Aggregate((c, n) => c.Or(n));

            return query.Where(expr);
        }
    }
}