using Microsoft.EntityFrameworkCore;
using XlsSeed;

namespace Demo.WebApp.Startup
{
    public static class Seed
    {
        public static void Run(DbContext dbContext)
        {
            var sm = new SeedManager();
            var sql = sm.GetSql("./seed.xlsx");
            dbContext.Database.ExecuteSqlCommand(sql);
        }
    }
}