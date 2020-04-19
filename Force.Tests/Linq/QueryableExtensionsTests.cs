using System.Linq;
using Force.Linq;
using Force.Tests.Cqrs;
using Force.Tests.Infrastructure;
using Force.Tests.Infrastructure.Context;
using Xunit;

namespace Force.Tests.Linq
{
    public class QueryableExtensionsTests: DbContextFixtureTestsBase
    {
        public QueryableExtensionsTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
        {
        }
        
        private static readonly string[] Strings =  {"1", "2", "3"};

        public IQueryable<string> Queryable = Strings.AsQueryable();

        [Fact]
        public void FilterByConventions()
        {
            var res = DbContext
                .Products
                .FilterByConventions(new PagedProductFilter());
        }
        
        [Fact]
        public void Filter()
        {
            var f = new PagedProductFilter();
            DbContext
                .Products
                .Filter(f)
                .FilterSortAndPaginate(f);
        }

        [Fact]
        public void FirstOrDefaultById_Projection()
        {
            var a = DbContext
                .Products
                .FirstOrDefaultById(0, x => new ProductListItem());
        }

        [Fact]
        public void FirstOrDefaultById()
        {
            DbContext
                .Products
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
            DbContext
                .Products
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