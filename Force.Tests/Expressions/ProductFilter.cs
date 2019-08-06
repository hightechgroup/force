using System.Linq;
using Force.Ddd;
using Force.Linq;

namespace Force.Tests.Expressions
{
    public class ProductFilter
        : IFilter<Product>
        , ISorter<Product>
    {
        public int? Id { get; set; }
        
        public string Name { get; set; }
        
        public string OrderBy { get; set; }
        
        public Spec<Product> Spec => new Spec<Product>(x => true);

        public IOrderedQueryable<Product> Sort(IQueryable<Product> queryable)
            => queryable.OrderBy(x => x.Id);
    }
}