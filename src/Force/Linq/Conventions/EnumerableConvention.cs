using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Force.Linq.Conventions.Filter;

namespace Force.Linq.Conventions
{
    public class EnumerableConvention : IFilterConvention
    {
        private static MethodInfo Contains = typeof(Enumerable)
            .GetMethods()
            .First(x => x.Name == "Contains" && x.GetParameters().Length == 2);

        public Expression BuildBody(MemberExpression propertyExpression, Expression valueExpression)
        {
            return Expression.Call(null, Contains.MakeGenericMethod(propertyExpression.Type), valueExpression, propertyExpression);
        }

        public bool CanConvert(Type predicateType, Type targetType) => 
            targetType.GetElementType() == predicateType;
    }
}