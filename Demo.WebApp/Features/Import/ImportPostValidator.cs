using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Force.Ddd;

namespace Demo.WebApp.Features.Import
{
    public class ImportPostValidator: IValidator<IEnumerable<ImportPost>>
    {
        public IEnumerable<ValidationResult> Validate(IEnumerable<ImportPost> obj, ValidationContext validationContext)
        {
            return new[] {new ValidationResult("error message")};
        }
    }
}