using System;
using System.Linq.Expressions;

namespace Force.Linq.Conventions.Search
{
    public abstract class SearchConvensionBase<T> : ISearchConvention
    {
        public abstract Expression BuildBody(MemberExpression propertyExpression, Expression valueExpression);

        public virtual bool CanConvert(Type targetType, Type valueType) => 
            valueType == typeof(T) && targetType == typeof(T);
    }
}