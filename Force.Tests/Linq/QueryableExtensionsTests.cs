using System.Linq;
using Force.Linq;
using Force.Tests.Cqrs;
using Force.Tests.Infrastructure;
using Force.Tests.Infrastructure.Context;
using Xunit;

namespace Force.Tests.Linq
{
    public class QueryableExtensionsTests
    {
        private static readonly string[] Strings =  {"1", "2", "3"};

        public IQueryable<string> Queryable = Strings.AsQueryable();

        public IQueryable<Product> ProductQueryable
            = new[] {new Product(null, "")
            {
            }}.AsQueryable();


        [Fact]
        public void FilterByConventions()
        {
            ProductQueryable
                .FilterByConventions(new PagedProductFilter());
        }
        
        [Fact]
        public void Filter()
        {
            var f = new PagedProductFilter();
            ProductQueryable
                .Filter(f)
                .FilterSortAndPaginate(f);
        }

        [Fact]
        public void FirstOrDefaultById_Projection()
        {
            var a = ProductQueryable
                .FirstOrDefaultById(0, x => new ProductListItem());
        }

        [Fact]
        public void FirstOrDefaultById()
        {
            ProductQueryable
                .WhereIf(false, x => true)
                .WhereIf(false, x => true, x => true)
                .WhereIfNotNull(null, x => true)
                .OrderById()
                .ById(0)
                .FirstOrDefaultById(0);
        }

        [Fact]
        public void LeftJoin()
        {
            ProductQueryable
                .LeftJoin(Queryable, 
                x => x.Name, 
                x => x, 
                (x,y) => x.Name + y)
                .ToList();
        }

        [Fact]
        public void ById()
        {
            
        }

        [Fact]
        public void WhereIf()
        {
            Queryable.WhereIf(true, x => true);
            Queryable.WhereIf(false, x => true);
            
            Queryable.WhereIf(true, x => true, x => false);
        }

        [Fact]
        public void OrderBy()
        {
            // try?
            Queryable.OrderBy("Length");
        }
        
        [Fact]
        public void OrderByDescending()
        {
            Queryable.OrderByDescending("Length");
        }
    }
}