using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Force.Ddd
{
    public class ValidatiorBuilder<T>: IEnumerable<ValidationResult>
    {
        private readonly T _obj;
        private readonly ValidationContext _validationContext;
        private List<ValidationResult> _validationResults = new List<ValidationResult>();

        public ValidatiorBuilder(T obj, ValidationContext validationContext = null)
        {
            _obj = obj;
            _validationContext = validationContext ?? new ValidationContext(_obj);
        }

        public ValidatiorBuilder<T> Validate(Func<T, bool> validationRule, string errorMessage)
        {
            if (!validationRule(_obj))
            {
                 _validationResults.Add(new ValidationResult(errorMessage));  
            }

            return this;
        }

        public IEnumerator<ValidationResult> GetEnumerator()
            => _validationResults.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _validationResults.GetEnumerator();
    }
}