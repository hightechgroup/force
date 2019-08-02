using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Force.Linq
{
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
}