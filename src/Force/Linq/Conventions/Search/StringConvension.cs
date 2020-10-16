using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Force.Linq.Conventions.Search
{
    public class StringConvension : SearchConvensionBase<string>
    {
        public override Expression BuildBody(MemberExpression propertyExpression, Expression valueExpression)
        {
            return Expression.Call(Expression.Call(propertyExpression, MethodInfos.ToLower),
                propertyExpression.Member.GetCustomAttribute<SearchByAttribute>()
                    ?.SearchKind == SearchKind.Contains
                    ? MethodInfos.Contains
                    : MethodInfos.StartsWith,
                valueExpression);
        }
    }
}