using WebApplication.Models;

namespace WebApplication.Features.Category
{
    public class Product: HasNameBase
    {
        public decimal Price { get; set; }
    }
}