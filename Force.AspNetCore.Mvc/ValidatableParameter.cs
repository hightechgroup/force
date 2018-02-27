using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Force.AspNetCore.Mvc
{
    public abstract class ValidatableParameter: IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}