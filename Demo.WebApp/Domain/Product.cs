using System;

namespace Demo.WebApp.Domain
{
    public class Product : NamedEntityBase
    {
        public Product(int id, string name, Category category) : base(id, name)
        {
            Category = category ?? throw new ArgumentNullException(nameof(category));
        }
        
        public Category Category { get; protected set; }
    }
}