using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SimpleInjector;
using Demo.WebApp.Infrastructure;
using Force.AutoMapper;
using Force.Cqrs;

namespace Demo.WebApp.Startup
{
    public static class RegistrationHelper
    {
        public static void RegisterQueryHandlers(this Container c, params Assembly[] assemblies)
        {
            var registrations = assemblies
                .SelectMany(x => x.GetExportedTypes())
                .Where(x => x.GetInterfaces()
                    .Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))
                .ToList();
            
            foreach (var reg in registrations)
            {
                var interfaces = reg.GetInterfaces()
                    .Where(x => x.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))
                    .ToList();

                foreach (var intr in interfaces)
                {
                    c.Register(intr, reg, Lifestyle.Transient);
                }                
            }
            
            var queryTypes = assemblies
                .SelectMany(x => x.GetExportedTypes())
                .Where(x => x.GetInterfaces()
                    .Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IQuery<>)))
                .ToList();

            foreach (var reg in queryTypes)
            {
                var interfaces = reg.GetInterfaces()
                    .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IQuery<>))
                    .ToList();
                
                foreach (var intr in interfaces)
                {
                    var returnType = intr.GetGenericArguments().First();
                    var unwrapedReturnType = returnType;
                    // not Task or Enumerable
                    if (!returnType.IsGenericType)
                    {
                        continue;
                    }

                    // enumerable
                    unwrapedReturnType = unwrapedReturnType.GetGenericArguments().First();
                    if (unwrapedReturnType.IsGenericType)
                    {
                        unwrapedReturnType = unwrapedReturnType.GetGenericArguments().First();
                    }

                    var entityType = unwrapedReturnType.GetCustomAttribute<AutoMapAttribute>()?.EntityType;
                    if (entityType == null)
                    {
                        continue;
                    }
                    
                    var impl = typeof(LinqQueryHandler<,,>).MakeGenericType(reg, entityType, unwrapedReturnType);
                    var handlerType = typeof(IQueryHandler<,>).MakeGenericType(reg, returnType);
                    c.Register(handlerType, impl, Lifestyle.Transient);
                }   
            }
        }
        
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
                c.Register(typeof(IQueryable<>).MakeGenericType(t), 
                    () => c.GetInstance<DbContext>().Set(t), Lifestyle.Scoped);    
            }
        }
    }
}