using System.ComponentModel.DataAnnotations;
using Force.Ddd;

namespace Force.Tests.Infrastructure.Context
{
    [Display(Name = "Product List")]
    public class ProductListItem: HasIdBase
    {
        public string Name { get; set; }
        
        public string CategoryName { get; set; }
    }
}