using Force.Ddd;
using Force.Tests.Infrastructure.Context;

namespace Force.Tests.Expressions
{
    public class Product : HasIdBase
    {
        public Category Category { get; set; }
        
        public string Name { get; set; }
    }
}