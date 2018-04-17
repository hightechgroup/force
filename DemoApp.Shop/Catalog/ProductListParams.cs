namespace DemoApp.Shop.Catalog
{
    public class ProductListParams
    {
        public bool? CategoryIsActive { get; set; }
        
        public string Name { get; set; }

        public decimal? Price { get; set; } 
        
        public string OrderBy { get; set; }
    }
}