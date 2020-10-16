using System;
using System.Collections.Generic;
using System.Linq;

namespace Force.Linq.Conventions
{
    public abstract class ConventionsBase<T> where T: IConvention
    {
        protected List<T> Conventions = new List<T>();

        internal T GetConvention(Type targetType, Type valueType) =>
            Conventions
                .FirstOrDefault(x =>
                    x.CanConvert(targetType, valueType));
    }
}