using System.Linq;
using Force.Cqrs;
using Force.Ddd;
using Force.Linq;

namespace Force.Tests.Infrastructure.Context
{
    public class ProductFilter : FilterQuery<Product>
    {
        public int? Id { get; set; }
        
        public string Name { get; set; }
        
        public string OrderBy { get; set; }
    }
}