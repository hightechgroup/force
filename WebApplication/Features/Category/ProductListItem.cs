using Force.Ddd;
using WebApplication.Models;

namespace WebApplication.Features.Category
{
    public class ProductListItem: HasNameBase
    {
        public override string ToString() => $"Product {Id}: {Name}";
        
        public decimal Price { get; set; }
    }
}