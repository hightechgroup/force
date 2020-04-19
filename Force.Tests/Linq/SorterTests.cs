using System;
using System.Linq;
using Force.Linq;
using Force.Tests.Infrastructure.Context;
using Xunit;

namespace Force.Tests.Linq
{
    public class SorterTests: DbContextFixtureTestsBase
    {
        public SorterTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
        {
        }
        
        [Fact]
        public void New()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var s = new Sorter<string>("Wrong");
            });
        }
        
        [Fact]
        public void TryParse_PropertyExists_AscNotNull()
        {
            Assert.True(Sorter<string>.TryParse("Length", out var s));
            Assert.NotNull(s);
            Assert.True(s.IsAsc);
        }
        
        [Fact]
        public void TryParse_PropertyExists_DescNotNull()
        {
            Assert.True(Sorter<string>.TryParse("Length Desc", out var s));
            Assert.NotNull(s);
            Assert.False(s.IsAsc);
        }


        [Theory]
        [InlineData("Length W")]
        [InlineData("Abyrwalg")]
        public void TryParse_PropertyDoesntExist_Null(string propertyName)
        {
            Assert.False(Sorter<string>.TryParse(propertyName, out var s));
            Assert.Null(s);
        }
        

        [Fact]
        public void Sort()
        {
            var s = new Sorter<Product>("Id", false);
            var sorted = s
                .Sort(DbContext.Products)
                .ToList();

            Assert.True(sorted.Count > 2);
            var flag = true;
            for (var i = 0; i < sorted.Count - 2; i++)
            {
                if (sorted[i].Id < sorted[i + 1].Id)
                {
                    flag = false;
                    break;
                }
            }
            
            Assert.True(flag);
        }
    }
}