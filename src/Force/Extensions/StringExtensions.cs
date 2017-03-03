using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Force.Extensions
{
    ///<summary>
    /// Класс свойств расширений для коллекций строк
    ///</summary>
    [PublicAPI]
    public static class StringExtensions
    {
        ///<summary>
        /// Объединяет коллекцию строк в одну строку использую разделитель
        ///</summary>
        ///<param name="source"></param>
        ///<param name="separator"></param>
        ///<returns></returns>
        [PublicAPI]
        public static string Join(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source);
        }

        [PublicAPI]
        public static bool Contains(this string input, string value, StringComparison comparisonType)
        {
            if (!string.IsNullOrEmpty(input))
            {
                return input.IndexOf(value, comparisonType) != -1;
            }

            return false;
        }


        [PublicAPI]
        public static bool LikewiseContains(this string input, string value)
        {
            return Contains(input, value, StringComparison.CurrentCulture);
        }

        [PublicAPI]
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

        [PublicAPI]
        public static string ToUnderscoreCase(this string str)
        {
            return string
                .Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()))
                .ToLower();
        }
    }
}
