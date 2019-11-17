using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Force.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            var type = value.GetType();
            var member = type.GetMember(value.ToString());

            var displayName = (DisplayAttribute)member[0]
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .FirstOrDefault();

            return displayName != null 
                ? displayName.Name 
                : SplitCamelCase(value.ToString());
        }
        
        internal static string SplitCamelCase(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", 
                System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }
    }
}