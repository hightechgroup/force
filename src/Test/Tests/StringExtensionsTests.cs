using System;
using System.Linq;
using CostEffectiveCode.Extensions;
using Xunit;

namespace CostEffectiveCode.Tests
{
    public class StringExtensionsTests
    {
        [Fact]
        public void Join_JoinStrings()
        {
            var strings = new[]
            {
                "a", "b", "c"
            };

            Assert.Equal("a,b,c", strings.Join(","));
        }

        [Fact]
        public void ToString_OneTwoFive()
        {
            var strs = new[] {1, 2, 5, 10, 20, 100}
                .Select(x => x.ToString("код", "кода", "кодов"))
                .ToArray();

            Assert.True(strs[0] == "код");
            Assert.True(strs[1] == "кода");
            Assert.True(strs[2] == "кодов");
        }

        [Fact]
        public void Contains_Null_False()
        {
            string nullStr = null;
            // ReSharper disable once PossibleNullReferenceException
            Assert.False(nullStr.Contains("smth", StringComparison.CurrentCultureIgnoreCase));
        }

        [Fact]
        public void Contains_NotNull_False()
        {
            var str = "eklmn";
            // ReSharper disable once PossibleNullReferenceException
            Assert.False(str.Contains("smth", StringComparison.CurrentCultureIgnoreCase));
        }

        [Fact]
        public void Contains_NotNull_True()
        {
            var str = "eklmn-smth";
            // ReSharper disable once PossibleNullReferenceException
            Assert.True(str.Contains("smth", StringComparison.CurrentCultureIgnoreCase));
        }

        [Fact]
        public void ToUnderscoreCase_True()
        {
            var toUnderscore = "CamelCase";
            Assert.Equal("camel_case", toUnderscore.ToUnderscoreCase());
        }

        [Fact]
        public void LikewiseContains_True()
        {
            var str = "eklmn-smth";
            // ReSharper disable once PossibleNullReferenceException
            Assert.True(str.LikewiseContains("smth"));
        }
    }
}
