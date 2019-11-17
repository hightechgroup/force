using System.ComponentModel.DataAnnotations;
using Force.Ddd;
using Xunit;

namespace Force.Tests.Ddd
{
    public class ValidationResultExtensionTests
    {
        [Fact]
        public void A()
        {
            ValidationResult[] results = new[]
            {
                ValidationResult.Success
            };

            results.IsValid();
        }
    }
}