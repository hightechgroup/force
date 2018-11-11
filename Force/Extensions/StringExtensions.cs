using System;
using System.Collections.Generic;
using System.Linq;

namespace Force.Extensions
{
    ///<summary>
    /// String class extensions
    ///</summary>
    public static class StringExtensions
    {
        public static string NullIfEmpty(this string str)
            => string.IsNullOrEmpty(str) ? null : str;
        
        public static string SplitCamelCase(this string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", 
                System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }
        
        /// <summary>
        /// Indicates whether the specified string not null or an empty string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static string Join(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source);
        }            

        public static bool Contains(this string input, string value, StringComparison comparisonType)
        {
            if (!string.IsNullOrEmpty(input))
            {
                return input.IndexOf(value, comparisonType) != -1;
            }

            return false;
        }

        public static bool LikewiseContains(this string input, string value)
        {
            return Contains(input, value, StringComparison.CurrentCulture);
        }

        public static string ToString(this int value, string oneForm, string twoForm, string fiveForm)
        {
            var significantValue = value%100;

            if (significantValue >= 10 && significantValue <= 20)
                return $"{value} {fiveForm}";

            var lastDigit = value%10;
            switch (lastDigit)
            {
                case 1:
                    return $"{oneForm}";
                case 2:
                case 3:
                case 4:
                    return $"{twoForm}";
            }

            return $"{fiveForm}";

        }

        public static string ToUnderscoreCase(this string str)
        {
            return string
                .Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()))
                .ToLower();
        }
    }
}
