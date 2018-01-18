using System;
using System.Collections.Concurrent;
using System.Linq;
using FastMember;
using Force.Demo.Data;
using Force.Demo.Domain;
using Force.Demo.Web.LinqToDb;
using LinqToDB.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Force.Demo.Web
{
    public class Startup
    {
        private static Category[] _categories = new Category[]
        {
            new Category("Смартфоны", "smartphones")
        };
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMiniProfiler()
                .AddEntityFramework();
            
            services.AddMvc();
            services.AddDbContext<DemoContext>(options => options.UseSqlServer(DemoContext.ConnectionString));
            services.AddScoped(x => new DemoConnection());
            services.AddScoped<IQueryable<Category>>(x =>
            {
                var dbContext = x.GetService<DemoContext>();
                return dbContext.Categories;
            });

            services.AddScoped<Func<Type, object, bool>>(x =>
            {
                bool Func(Type t, object o)
                {
                    var dbContext = x.GetService<DemoContext>();
                    return dbContext.Find(t, o) != null;
                };

                return Func;
            });
            
            services.AddScoped<Func<Type, object, object>>(x =>
            {
                object Func(Type t, object o)
                {
                    var typeAccessor = TypeAccessor.Create(t);
                    var dbContext = x.GetService<DemoContext>();
                    return dbContext.Find(t, o);
                };
                
                return Func;
            });
            
            DataConnection.DefaultSettings = new ForceSettings();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            
#if DEBUG
            app.UseDeveloperExceptionPage();
#endif
      
            app.UseMiniProfiler();
            app.UseMvc();

        }
    }
}