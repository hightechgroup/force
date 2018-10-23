using System.Collections.Generic;

namespace Force.Tests
{
    public class Category: NamedEntityBase
    {
        public Category(int id, string name) : base(id, name)
        {
        }
        
        public ICollection<Product> Products { get; protected set; } = new HashSet<Product>();
    }
}