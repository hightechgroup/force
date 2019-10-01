using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    [Display(Name = "Product!!!")]
    public class Product
    {
        public string Name { get; set; }
        
        public override string ToString() => Name;
    }
}