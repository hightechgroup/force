using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Force.Linq.Conventions
{
    public class EnumerableConvention : IFilterConvention
    {
        private static MethodInfo Contains = typeof(Enumerable)
            .GetMethods()
            .First(x => x.Name == "Contains" && x.GetParameters().Length == 2);
        
        public bool CanConvert(Type predicateType, Type targetType) => 
            targetType.GetElementType() == predicateType;

        public Expression BuildFilterBody(MemberExpression propertyExpression, Expression valueExpression)
        {
            return Expression.Call(null, Contains.MakeGenericMethod(propertyExpression.Type), valueExpression, propertyExpression);
        }
    }
}