using System.ComponentModel.DataAnnotations;

namespace DotNext.DddWorkshop.Areas.Products.Domain
{
    public class ProductPrice
    {
        [Required, Range(0, 100500)]
        public int Price { get; set; }
        
        [Range(0, 50)]
        public int DiscountPercent { get; set; }
    }
}