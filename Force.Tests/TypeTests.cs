using System.Collections.Generic;
using Force.Ddd;
using Force.Infrastructure;
using Xunit;

namespace Force.Tests
{
    public class SimpeValueObject : ValueObject<string>
    {
        public SimpeValueObject(string value) : base(value)
        {
        }
    }
    
    public class TypeTests
    {
        [Fact]
        public void Create()
        {
            var vo = Type<SimpeValueObject>.CreateInstance("string");
        }
    }
}