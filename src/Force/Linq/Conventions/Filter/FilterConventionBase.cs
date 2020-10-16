using System;
using System.Linq.Expressions;

namespace Force.Linq.Conventions.Filter
{
    public abstract class FilterConventionBase<T>: IFilterConvention
    {
        public bool CanConvert(Type predicateType, Type targetType)
            => predicateType == typeof(T) && targetType == typeof(T);

        public abstract Expression BuildBody(MemberExpression propertyExpression, Expression valueExpression);
    }
}