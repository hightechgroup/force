using System;
using System.ComponentModel.DataAnnotations;
using Force.Ddd;

namespace Force.Meta.Validation
{   
    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Field)]
    public class RefinementAttribute: ValidationAttribute
    {       
        public IValidator<object> Refinement { get; }

        public RefinementAttribute(Type refinmentType)
        {
            Refinement = (IValidator<object>)Activator.CreateInstance(refinmentType);
            ErrorMessage = ((IHasErrorMessage)Refinement).ErrorMessage;
        }

        public override bool IsValid(object value)
            => Refinement.Validate(value).IsValid();
    }
    
}