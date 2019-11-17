using Microsoft.AspNetCore.Mvc;

namespace DotNext.DddWorkshop.Areas.Products.Domain
{
    public class ChangePrice
    {
        public int ProductId { get; set; }
        
        public ProductPrice ProductPrice { get; set; }
        
    }
}