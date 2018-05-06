using System;
using System.ComponentModel.DataAnnotations;

namespace DemoApp.Domain
{
    public abstract class ProductBase : HasNameBase
    {
        protected ProductBase()
        {            
        }
        
        protected ProductBase(string name) : base(name)
        {
        }

        public decimal Price { get; protected set; }        
    }

    public class Product: ProductBase
    {
        [Required]
        public Category Category { get; protected set; }
        
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