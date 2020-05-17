using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Force.Ccc
{
    public interface IValidator<in T>
    {
        IEnumerable<ValidationResult> Validate(T obj);
    }    
    
    public static class ValidationResultExtensions
    {
        public static bool IsValid(this IEnumerable<ValidationResult> results)
            => results == null || results.All(x => x == ValidationResult.Success);        
    }
}