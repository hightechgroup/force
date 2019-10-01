using System.Linq;
using Force.Ddd;
using Force.Linq;

namespace Force.Tests.Infrastructure.Context
{
    public class ProductFilter
        : IFilter<Product>
        , ISorter<Product>
    {
        public int? Id { get; set; }
        
        public string Name { get; set; }
        
        public string OrderBy { get; set; }

        public IOrderedQueryable<Product> Sort(IQueryable<Product> queryable)
            => queryable.OrderBy(x => x.Id);

        public IQueryable<Product> Filter(IQueryable<Product> queryable)
        {
            throw new System.NotImplementedException();
        }
    }
}