using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Force.Ddd;
using Force.Extensions;
using Xunit;

namespace Force.Tests.Extensions
{
    public class ExtensionTests
    {
        [Fact]
        public void EitherOr()
        {
            var a = true;
            a.EitherOr(a, x => !x, x => x);
            a.EitherOr(x => true, x => !x, x => x);
            a.EitherOr(x => !x, x => x);
        }

        [Fact]
        public void ValidationResultsExtensions()
        {
            IEnumerable<ValidationResult> results = new List<ValidationResult>();
            results.IsValid();
        }
        
        [Fact]
        public void Merge()
        {
            
        }
    }
}