using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Force.Tests.Ddd
{
    [SuppressMessage("ReSharper", "NotAccessedVariable")]
    public class ValueObjectTests
    {
        private static TestValueObject vo1 = new("vo1");
        private static TestValueObject vo2 = new("vo2");
        private static TestValueObject vo3 = new("vo1");

        [Fact]
        public void Implicit()
        {
            string s = vo1;
        }

        // [Fact]
        // public void Equals_2()
        // {
        //     var vo = new StringValueObject();
        //     vo.Equals(vo);
        //     vo.Equals(null);
        //     var a = vo == vo;
        //     var b = vo != vo;
        //     
        //     vo.Equals(vo1);
        //
        //     StringValueObject vo2 = null;
        //     vo2 == vo2;
        //     vo2 != vo2;
        //     vo == vo2;
        // }
        
        [Fact]
        public void Equals_()
        {
            Assert.False(vo1.Equals(vo2));
            Assert.False(vo1 == vo2);
            Assert.True(vo1 != vo2);

            Assert.False(vo2.Equals(vo3));
            Assert.False(vo2 == vo3);
            Assert.True(vo2 != vo3);

            Assert.True(vo1.Equals(vo3));
            Assert.True(vo1 == vo3);
            Assert.False(vo1 != vo3);
        }

        [Fact]
        public void GetHashCode_()
        {
            Assert.Equal(vo1.GetHashCode(), vo3.GetHashCode());
            Assert.NotEqual(vo1.GetHashCode(), vo2.GetHashCode());
        }
    }
}