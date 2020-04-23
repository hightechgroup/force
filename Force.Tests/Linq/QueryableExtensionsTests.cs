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
        public void FilterByConventions_Empty()
        {
            var totalCount = DbContext
                .Products
                .Count();
        
            var res = DbContext
                .Products
                .FilterByConventions(new PagedProductFilter())
                .Count();
            
            Assert.Equal(totalCount, res);
        }
        
        [Fact]
        public void FilterSortAndPaginate()
        {
            var f = new PagedProductFilter()
            {
                Asc = false,
                Order = "Name",
                Page = 1,
                Take = 2
            };
                
            var res = DbContext
                .Products
                .Filter(f)
                .FilterSortAndPaginate(f);
            
            Assert.Equal(2, res.Count());
            Assert.Equal(DbContext.Products.Count(), res.Total);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void FirstOrDefaultById_Projection(int id)
        {
            var actual = DbContext
                .Products
                .FirstOrDefaultById(id, x => new ProductListItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    CategoryName = x.Category.Name
                });

            var expected = DbContext
                .Products
                .Where(x => x.Id == id)
                .Select(x => new ProductListItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    CategoryName = x.Category.Name
                })
                .FirstOrDefault();
            
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.CategoryName, actual.CategoryName);
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