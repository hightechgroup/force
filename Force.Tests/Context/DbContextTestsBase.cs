using Demo.WebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace Force.Tests
{
    public abstract class DbContextTestsBase
    {
        public static volatile bool IsInitialized = false; 
        
        public static object Locker = new object();
        
        protected readonly DemoAppDbContext DbContext;
            
        protected DbContextTestsBase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DemoAppDbContext>();
            optionsBuilder.UseInMemoryDatabase("Force");

            DbContext = new DemoAppDbContext(optionsBuilder.Options);

            
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