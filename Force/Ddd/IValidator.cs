using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Force.Ddd
{
    public interface IValidator<in T>
    {
        IEnumerable<ValidationResult> Validate(T obj);
    }
    
    public static class ValidationExtensions
    {
        public static bool IsValid(this IEnumerable<ValidationResult> results)
            => results == null || results.All(x => x == ValidationResult.Success);
        
        public static ValidationResult Validate<T>(this T obj, Func<T, bool> func, string errorMessage)
            => func(obj)
                ? ValidationResult.Success
                : new ValidationResult(errorMessage);


        public static ValidationResult[] Validate<T>(this T data, params Func<T, ValidationResult>[] funcs)
            => funcs
                .Select(x => x.Invoke(data))
                .Where(x => x != ValidationResult.Success)
                .ToArray();
    }
}