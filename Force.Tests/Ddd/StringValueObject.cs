using System.Collections.Generic;
using Force.Ddd;

namespace Force.Tests.Ddd
{
    public class StringValueObject: ValueObject
    {
        public string Value { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}