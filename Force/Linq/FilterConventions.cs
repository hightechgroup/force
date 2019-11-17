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
            if (_instance != null)
            {
                throw new InvalidOperationException("Filter conventions are already initialized");
            }
            
            var conventions = new Dictionary<Type, Func<MemberExpression, Expression, Expression>>()
            {
                [typeof(string)] = (p, v)
                    => Expression.Call(Expression.Call(p, ToLower), 
                        p.Member.GetCustomAttribute<SearchByAttribute>()
                            ?.SearchKind == SearchKind.Contains
                            ? Contains : StartsWith, 
                        v)
            };

            setConventions?.Invoke(conventions);

            _instance = new FilterConventions {_filters = conventions};
            return Instance;
        }
        
        private static MethodInfo Contains = typeof(string)
            .GetMethod("Contains", new[] {typeof(string)});
        
        private static MethodInfo StartsWith = typeof(string)
            .GetMethod("StartsWith", new[] {typeof(string)});
        
        private static MethodInfo ToLower = typeof(string)
            .GetMethod("ToLower", new Type[]{});


        private static FilterConventions _instance;
        
        public static FilterConventions Instance
        {
            get
            {
                if (_instance == null)
                {
                    Initialize();
                }
                
                return _instance;
            }
        }
        
        private Dictionary<Type, Func<MemberExpression, Expression, Expression>> _filters
            = new Dictionary<Type, Func<MemberExpression, Expression, Expression>>();
            
        internal Func<MemberExpression, Expression, Expression> this[Type key] =>
            _filters.ContainsKey(key)
                ? _filters[key]
                : Expression.Equal;
    }
}