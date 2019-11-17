using DotNext.DddWorkshop.Models;

namespace DotNext.DddWorkshop.Areas.Products.Domain
{
    public class Product: HasNameBase
    {
        public static readonly ProductSpecs Specs = new ProductSpecs();
        
        public int Price { get; set; }
        
        public int DiscountPercent { get; set; }

        public decimal DiscountedPrice => Price - Price * DiscountPercent;

        public override string ToString() => $"{base.ToString()} ${Price}";
    }

    public interface IHasDiscount
    {
        int Price { get; set; }
        
        int DiscountPercent { get; set; }

        decimal DiscountedPrice() => Price - Price / 100 * DiscountPercent;
    }
    
}