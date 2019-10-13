using System;

namespace Force.Reflection
{
    public static class TypeExtensions
    {
        public static bool TryGetValue(this object obj, string propertyName, out object value)
        {
            value = obj
                    ?.GetType()
                    .GetProperty(propertyName)
                    ?.GetValue(obj);

            return value != default;
        }
    }
}