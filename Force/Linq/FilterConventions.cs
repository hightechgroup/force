using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Force.Linq
{
    public class FilterConventions
    {
        private FilterConventions() {}

        public static FilterConventions Initialize(
            Action<Dictionary<Type, Func<MemberExpression, Expression, Expression>>> setConventions = null)
        {
            if (Instance != null)
            {
                throw new InvalidOperationException("Filter conventions are already initialized");
            }
            var conventions = new Dictionary<Type, Func<MemberExpression, Expression, Expression>>()
            {
                [typeof(string)] = (p, v)
                    => Expression.Call(Expression.Call(p, ToLower), StartsWith, v)
            };

            setConventions?.Invoke(conventions);

            Instance = new FilterConventions {_filters = conventions};
            return Instance;
        }

        private static MethodInfo StartsWith = typeof(string)
            .GetMethod("StartsWith", new[] {typeof(string)});
        
        private static MethodInfo ToLower = typeof(string)
            .GetMethod("ToLower", new Type[]{});
        
        
        public static FilterConventions Instance { get; private set; }
        
        private Dictionary<Type, Func<MemberExpression, Expression, Expression>> _filters
            = new Dictionary<Type, Func<MemberExpression, Expression, Expression>>();
            
        internal Func<MemberExpression, Expression, Expression> this[Type key] =>
            _filters.ContainsKey(key)
                ? _filters[key]
                : Expression.Equal;
    }
}