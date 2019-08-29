using System.ComponentModel.DataAnnotations;
using Force.Extensions;
using Xunit;

namespace Force.Tests.Extensions
{
    public enum TestEnum
    {
        One,
        [Display(Name = "Three")]
        Two
    }
    
    public class EnumExtensionsTests
    {
        [Fact]
        public void GetDisplayName()
        {
            TestEnum.One.GetDisplayName();
            TestEnum.Two.GetDisplayName();
        }
    }
}