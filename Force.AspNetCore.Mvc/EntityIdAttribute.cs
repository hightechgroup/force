using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;

namespace Force.AspNetCore.Mvc
{
    
    public class EntityIdAttribute: ValidationAttribute  
    {
        private readonly Type _entityType;

        public EntityIdAttribute(Type entityType)
        {
            _entityType = entityType;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var checker = validationContext.GetService(typeof(Func<Type, object, bool>))
                as Func<Type, object, bool> ?? throw new InvalidOperationException("You must register Func<Type, object, bool>");
            
            return checker(_entityType, value)
                ? ValidationResult.Success
                : new ValidationResult($"Entity with id {value} is not found");
        }
            
    }
}