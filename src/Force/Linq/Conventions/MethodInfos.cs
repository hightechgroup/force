using System;
using System.Reflection;

namespace Force.Linq.Conventions
{
    internal static class MethodInfos
    {
        internal static MethodInfo ToUpper = typeof(string)
            .GetMethod("ToUpper", new Type[] { });

        internal static MethodInfo ToLower = typeof(string)
            .GetMethod("ToLower", new Type[] { });

        internal static MethodInfo Contains = typeof(string)
            .GetMethod("Contains", new[] {typeof(string)});

        internal static MethodInfo StartsWith = typeof(string)
            .GetMethod("StartsWith", new[] {typeof(string)});
    }
}