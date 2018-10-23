using System;
using System.Collections.Generic;

namespace Force.Tests
{
    public class Product : NamedEntityBase
    {
        public Product(int id, string name, Category category) : base(id, name)
        {
            Category = category ?? throw new ArgumentNullException(nameof(category));
        }
        
        public Category Category { get; protected set; }
        
        public ICollection<SaleItem> SaleItems { get; protected set; } = new HashSet<SaleItem>();
    }
}