using System;
using System.Linq.Expressions;

namespace Force.Linq.Conventions
{
    public interface IConvention
    {
        Expression BuildBody(MemberExpression propertyExpression, Expression valueExpression);
        
        bool CanConvert(Type targetType, Type valueType);
    }
}