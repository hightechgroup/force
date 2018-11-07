using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Force.Ddd
{
    public interface IValidator<in T>
    {
        IEnumerable<ValidationResult> Validate(T obj, ValidationContext validationContext);
    }    
    
    public static class ValidationExtensions
    {
        public static IEnumerable<ValidationResult> Validate<T>(this IValidator<T> validator, T obj) =>
            validator.Validate(obj, new ValidationContext(obj));       
        
        public static bool IsValid(this IEnumerable<ValidationResult> results)
            => results == null || results.All(x => x == ValidationResult.Success);
        

        public static IEnumerable<ValidationResult> Validate<T>(this T obj, ValidationContext validationContext = null)
        {            
            if (validationContext == null)
            {
                validationContext = new ValidationContext(obj, null, items: null);
            }
            

            var results = new List<ValidationResult>();
            
            Validator.TryValidateObject(
                obj, validationContext, results, 
                validateAllProperties: true
            );

            return results;
        }

        public static IEnumerable<ValidationResult> Validate<T>(this IEnumerable<IValidator<T>> validators,
            T obj, ValidationContext validationContext = null)
        {
            if (validationContext == null)
            {
                validationContext = new ValidationContext(obj);
            }
            
            return validators
                .SelectMany(x => x.Validate(obj, validationContext))
                .ToList(); 
        }

    }
}