using Microsoft.EntityFrameworkCore;

namespace Force.Tests.Infrastructure.Context
{
    public class DbContextFixture
    {
        public const string FirstCategoryName = "C1";

        public const string FirstProductName = "P1";

        public const string SecondProductName = "P2";

        public const string LastProductName = "PL";

        public const int LastProductId = 10;
        
        public TestsDbContext DbContext { get; }

        static DbContextFixture()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestsDbContext>();
            optionsBuilder.UseInMemoryDatabase("Force");
            var dbContext = new TestsDbContext(optionsBuilder.Options);
            
            var category = new Category(FirstCategoryName);
            dbContext.Products.Add(new Product(category, FirstProductName)
            {
                Id = 1,
            });
            
            dbContext.Products.Add(new Product(category, SecondProductName)
            {
                Id = 2,
            });
            
            dbContext.Products.Add(new Product(category, "P3")
            {
                Id = 3,
            });
            
            dbContext.Products.Add(new Product(category, LastProductName)
            {
                Id = 10,
            });
//            
//            dbContext.Products.Add(new Product()
//            {
//                Id = 2,
//                Name = "123"
//            });
//            
//            dbContext.Products.Add(new Product()
//            {
//                Id = 3,
//                Name = "Abc"
//            });

            dbContext.SaveChanges();
        }
        
        public DbContextFixture()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestsDbContext>();
            optionsBuilder.UseInMemoryDatabase("Force");
            DbContext = new TestsDbContext(optionsBuilder.Options);
        }
    }
}