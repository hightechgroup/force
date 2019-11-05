namespace DotNext.DddWorkshop.Areas.Products.Entities
{
    public class Product: HasNameBase
    {
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