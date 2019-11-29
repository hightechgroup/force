﻿using System;
using System.Collections.Generic;
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

        public static IOrderedQueryable<TSubject> OrderBy<TSubject>(this IQueryable<TSubject> query, string propertyName)
            => Conventions<TSubject>.Sort(query, propertyName);
    }

    public static class Conventions
    {
        public static ConventionalFilters Filters { get; } = new ConventionalFilters();
    }

    public class ConventionalFilters
    {
        private static MethodInfo StartsWith = typeof(string)
            .GetMethod("StartsWith", new[] {typeof(string)});

        private static Dictionary<Type, Func<MemberExpression, Expression, Expression>> _filters
            = new Dictionary<Type, Func<MemberExpression, Expression, Expression>>()
            {
                { typeof(string),  (p, v) => Expression.Call(p, StartsWith, v) }
            };
        
        internal ConventionalFilters()
        {            
        }

        public Func<MemberExpression, Expression, Expression> this[Type key]
        {
            get => _filters.ContainsKey(key)
                ? _filters[key]
                : Expression.Equal;
            set => _filters[key] = value ?? throw new ArgumentException(nameof(value));
        }
    }

    public static class Conventions<TSubject>
    {
        public static IOrderedQueryable<TSubject> Sort(IQueryable<TSubject> query, string propertyName)
        {
            (string, bool) GetSorting()
            {
                var arr = propertyName.Split('.');
                if (arr.Length == 1)
                    return (arr[0], false);
                var sort = arr[1];
                if (string.Equals(sort, "ASC", StringComparison.CurrentCultureIgnoreCase))
                    return (arr[0], false);
                if (string.Equals(sort, "DESC", StringComparison.CurrentCultureIgnoreCase))
                    return (arr[0], true);
                return (arr[0], false);
            }

            var (name, isDesc) = GetSorting();
            propertyName = name;

            var property = FastTypeInfo<TSubject>
                .PublicProperties
                .FirstOrDefault(x => string.Equals(x.Name, propertyName, StringComparison.CurrentCultureIgnoreCase));

            if (property == null)
                throw new InvalidOperationException($"There is no public property \"{propertyName}\" " +
                                                    $"in type \"{typeof(TSubject)}\"");

            var parameter = Expression.Parameter(typeof(TSubject));
            var body = Expression.Property(parameter, propertyName);

            var lambda = FastTypeInfo<Expression>
                .PublicMethods
                .First(x => x.Name == "Lambda");

            lambda = lambda.MakeGenericMethod(typeof(Func<,>)
                .MakeGenericType(typeof(TSubject), property.PropertyType));

            var expression = lambda.Invoke(null, new object[] {body, new[] {parameter}});

            var methodName = isDesc ? "OrderByDescending" : "OrderBy";

            var orderBy = typeof(Queryable)
                .GetMethods()
                .First(x => x.Name == methodName && x.GetParameters().Length == 2)
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
                    var property = Expression.Property(parameter, x.Property);
                    Expression value = Expression.Constant(x.Value);

                    value = Expression.Convert(value, property.Type);
                    var bodyExpr = Conventions.Filters[property.Type](property, value);

                    return bodyExpr;
                })
                .ToArray();

            if (!props.Any())
            {
                return query;
            }

            var body = composeKind == ComposeKind.And
                ? props.Aggregate((c, n) => Expression.AndAlso(c, n))
                : props.Aggregate((c, n) => Expression.OrElse(c, n));

            var expr = Expression.Lambda<Func<TSubject, bool>>(body, parameter);

            return query.Where(expr);
        }
    }
}