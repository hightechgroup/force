using System.Linq;
using Force.Tests.Infrastructure.Context;

namespace Force.Tests.Cqrs
{
    public class ProductFixture
    {
        public const string FirstProductName = "FirstProductName";
        
        private readonly Category _category = new Category("Sport");
        
        public Product[] Products { get; }

        public IQueryable<Product> ProductQueryable => Products.AsQueryable();
        
        public ProductFixture()
        {
            Products = new[]
            {
                new Product(_category, FirstProductName)
            };
        }
    }
}