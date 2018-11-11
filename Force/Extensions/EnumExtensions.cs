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
            if (value == null)
            {
                return null;                
            }
            
            var type = value.GetType();
            var member = type.GetMember(value.ToString());
            if (!member.Any()) return "";

            var displayName = (DisplayAttribute)member[0]
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .FirstOrDefault();

            return displayName != null ? displayName.Name : value.ToString().SplitCamelCase();
        }
    }
}