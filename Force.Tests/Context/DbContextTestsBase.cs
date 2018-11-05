using Microsoft.EntityFrameworkCore;

namespace Force.Tests
{
    public abstract class DbContextTestsBase
    {
        public static volatile bool IsInitialized = false; 
        
        public static object Locker = new object();
        
        protected readonly TestDbContext DbContext;
            
        protected DbContextTestsBase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
            optionsBuilder.UseInMemoryDatabase("Force");

            DbContext = new TestDbContext(optionsBuilder.Options);

            
            if (!IsInitialized)
            {
                lock (Locker)
                {
                    if (!IsInitialized)
                    {
                        DbContext.SaveChanges();
                    }

                    IsInitialized = true;
                }
            }
        }
    }
}