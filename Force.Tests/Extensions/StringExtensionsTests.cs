using System.Linq;
using Force.Extensions;
using Xunit;

namespace Force.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Fact]
        public void A()
        {
            ""
                .NullIfEmpty()
                .PipeTo(x => " ")
                .ToUnderscoreCase()
                .SplitCamelCase()
                .HasValue();
        }
    }
}