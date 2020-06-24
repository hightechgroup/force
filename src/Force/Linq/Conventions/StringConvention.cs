using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Force.Linq.Conventions
{
    public class StringConvention: FilterConventionBase<String>
    {
        private static MethodInfo ToLower = typeof(string)
            .GetMethod("ToLower", new Type[]{});

        private static MethodInfo Contains = typeof(string)
            .GetMethod("Contains", new[] {typeof(string)});
        
        private static MethodInfo StartsWith = typeof(string)
            .GetMethod("StartsWith", new[] {typeof(string)});

        public override Expression BuildFilterBody(MemberExpression propertyExpression, Expression valueExpression)
        {
            return Expression.Call(Expression.Call(propertyExpression, ToLower),
                propertyExpression.Member.GetCustomAttribute<SearchByAttribute>()
                    ?.SearchKind == SearchKind.Contains
                    ? Contains
                    : StartsWith,
                valueExpression);
        }
    }
}