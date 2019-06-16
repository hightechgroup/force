using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Force.Ddd;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.WebApp.Domain.Validators
{
    public class ExistingEmailValidator<T>: IValidator<T>
        where T: IHasEmail
    {
        public IEnumerable<ValidationResult> Validate(T obj, ValidationContext validationContext)
            => null;

        /*
        validationContext
        .GetService<IQueryable<Account>>()
        .Any(x => x.Email.ToString() == obj.Email)
            ? new[] {new ValidationResult("Email already exists", new[] {"Email"})}
            : null;
            */
    }
}