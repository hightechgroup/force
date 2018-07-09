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
        

        public static IEnumerable<ValidationResult> Validate<T>(this T obj)
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
            => validators.SelectMany(x => x.Validate(obj));

    }
}