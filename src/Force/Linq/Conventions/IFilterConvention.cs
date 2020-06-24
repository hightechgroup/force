using System;
using System.Linq.Expressions;

namespace Force.Linq.Conventions
{
    public interface IFilterConvention
    {
        bool CanConvert(Type predicateType, Type targetType);

        Expression BuildFilterBody(MemberExpression propertyExpression, Expression valueExpression);
    }
}