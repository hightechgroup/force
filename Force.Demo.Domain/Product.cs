using System;
using System.ComponentModel.DataAnnotations;
using Force.Ddd;
using LinqToDB.Mapping;
namespace Force.Demo.Domain
{
    [Table(Name = "Products")]
    public class Product: HasNameBase
    {
        public decimal Price { get; protected set; }

        [Required]
        public virtual Category Category { get; set; }
        
        public Product()
        {
            
        }
        
        public Product(string name, decimal price, Category category): base(name)
        {
            Price = price;
            Category = category ?? throw new ArgumentNullException(nameof(category));
        }
    }
}