using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Force.Ddd
{
    public static class ValidationExtensions
    {
        public static string Check<T>(this T obj, Func<T, bool> func, string message)
            => func(obj) ? null : message;


        public static string[] Check<T>(this T data, params Func<T, string>[] funcs)
            => funcs
                .Select(x => x.Invoke(data))
                .Where(x => x != null)
                .ToArray();

        public static ValidationResult GetResult<T>(this T data, params Func<T, string>[] funcs)
        {
            var checkResult = data.Check(funcs);
            return checkResult.Any()
                ? new ValidationResult(checkResult.Join(","))
                : ValidationResult.Success;
        }

        public static string Join(this IEnumerable<string> strings, string delimiter)
            => strings.Aggregate((c, n) => $"{c}{delimiter}{n}");
    }

    public interface IValidator<in T>
    {
        ValidationResult Validate(T obj);
    }
}