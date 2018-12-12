using System.Linq;
using Microsoft.EntityFrameworkCore;
using SimpleInjector;
using Demo.WebApp.Infrastructure;

namespace Demo.WebApp.Startup
{
    public static class RegistrationHelper
    {
        public static void RegisterQueryables<T>(this Container c)
            where T: DbContext
        {
            var types = typeof(T)
                .GetProperties()
                .Where(x => x.PropertyType.IsGenericType &&
                            x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .Select(x => x.PropertyType.GetGenericArguments().First())
                .ToList();

            foreach (var t in types)
            {
                c.Register(typeof(IQueryable<>).MakeGenericType(t), () => c.GetInstance<DbContext>().Set(t));    
            }
        }
    }
}