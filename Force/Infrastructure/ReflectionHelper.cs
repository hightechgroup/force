using System;
using System.Linq;
using System.Reflection;

namespace Force.Infrastructure
{
    public static class ReflectionHelper<T>
    {
        private static Attribute[] _attributes;

        private static PropertyInfo[] _properties;
            
        static ReflectionHelper()
        {
            var type = typeof(T);
            _attributes = type.GetCustomAttributes().ToArray();
            
            _properties = type
                .GetProperties()
                .Where(x => x.CanRead && x.CanWrite)
                .ToArray();
        }

        public static PropertyInfo[] PublicProperties => _properties;

        public static Attribute[] Attributes => _attributes;

    }
}