using System.ComponentModel.DataAnnotations;
using Force.Ddd;

namespace Force.Tests.Infrastructure.Context
{
    [Display(Name = "Product")]
    public class Product : HasIdBase
    {
        public Category Category { get; set; }
        
        public string Name { get; set; }
    }
}