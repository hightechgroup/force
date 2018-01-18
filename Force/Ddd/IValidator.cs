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
        public static Result<T> ToResult<T>(this IEnumerable<ValidationResult> validationResults)
            => Result.Fail<T>(new ValidationFailure(validationResults));
        
        public static bool IsValid(this IEnumerable<ValidationResult> results)
            => results == null || results.All(x => x == ValidationResult.Success);
        

        public static IEnumerable<ValidationResult> Validate(this object obj)
        {
            var context = new ValidationContext(obj, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            
            Validator.TryValidateObject(
                obj, context, results, 
                validateAllProperties: true
            );

            return results;
        }

        public static IEnumerable<ValidationResult> Validate<T>(this T obj, params IValidator<T>[] validators)
            => validators.SelectMany(x => x.Validate(obj)).ToArray();
        
        public static IEnumerable<ValidationResult> Validate<T>(this T obj, params Func<T, ValidationResult>[] funcs)
            => funcs
                .Select(x => x.Invoke(obj))
                .Where(x => x != ValidationResult.Success)
                .ToArray();
        
        public static Result<T> ValidateToResult<T>(this T obj, params Func<T, ValidationResult>[] funcs)
            => obj.Validate(funcs).ToResult<T>();

        public static Result<T> ValidateToResult<T>(this T obj)
            => Validate((object)obj).ToResult<T>();
    }
}