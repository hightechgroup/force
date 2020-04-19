using System.ComponentModel.DataAnnotations;
using Force.Ddd;
using Xunit;

namespace Force.Tests.Extensions
{
    public class ValidationResultExtensionTests
    {
        [Fact]
        public void Success()
        {
            ValidationResult[] results = {
                ValidationResult.Success,
                ValidationResult.Success,
            };

            Assert.True(results.IsValid());
        }
        
        [Fact]
        public void Failure()
        {
            ValidationResult[] results = {
                ValidationResult.Success,
                new ValidationResult("Error"), 
            };

            Assert.False(results.IsValid());
        }
    }
}