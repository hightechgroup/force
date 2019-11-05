using WebApplication.Extensions;
using WebApplication.Features.Category;
using WebApplication.Models;

namespace WebApplication.Features
{
    public class Product: HasNameBase
    {
        public static ProductSpecs Specs = new ProductSpecs();
        
        public decimal Price { get; set; }
        
        public decimal DiscountPercent { get; set; }

        public decimal DiscountedPrice => Price - Price * DiscountedPrice;
    }

    public interface IHasDiscount
    {
        decimal Price { get; set; }
        
        decimal DiscountPercent { get; set; }

        decimal DiscountedPrice() => Price - Price / 100 * DiscountPercent;
    }
    
}