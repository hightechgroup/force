using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Force.Extensions;

namespace Force.Ddd
{
    public class ValidationFailure: Failure
    {
        public ValidationFailure(IEnumerable<ValidationResult> validationResults) 
            : base(ValidationResultsToStrings(validationResults))
        {
            Data = new ReadOnlyDictionary<string, object>(validationResults.ToDictionary(
                x => x.MemberNames.Join(","),
                x => (object)x.ErrorMessage));
        }

        private static string ValidationResultsToStrings(IEnumerable<ValidationResult> validationResults)
        {
            if (validationResults == null || !validationResults.Any())
            {
                throw new ArgumentException(nameof(validationResults));
            }

            return validationResults
                .Select(x => x.ErrorMessage)
                .Join(Environment.NewLine);
        }
    }
}