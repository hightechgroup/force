using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using DemoApp.Data;
using DemoApp.Domain;
using Force.Cqrs;
using Force.Ddd;
using Force.Ddd.Pagination;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;

namespace Force.Demo.Web
{
    public class Startup
    {
        private Container container = new Container();
        
        private void IntegrateSimpleInjector(IServiceCollection services) {
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IControllerActivator>(
                new SimpleInjectorControllerActivator(container));
            services.AddSingleton<IViewComponentActivator>(
                new SimpleInjectorViewComponentActivator(container));

            services.EnableSimpleInjectorCrossWiring(container);
            services.UseSimpleInjectorAspNetRequestScoping(container);
        }
        
        private void InitializeContainer(IApplicationBuilder app) {
            // Add application presentation components:
            container.RegisterMvcControllers(app);
            container.RegisterMvcViewComponents(app);

            // Add application services. For instance:
           
            // Cross-wire ASP.NET services (if any). For instance:
            container.CrossWire<ILoggerFactory>(app);
            //container.CrossWire<DbContext>(app);
//            container.RegisterConditional(
//                typeof(IQuery<,>), typeof(EfMapsterQuery<,>),
//                Lifestyle.Scoped,
//                c => !c.Handled
//                     && typeof(IPaging).IsAssignableFrom(c.ServiceType.GetGenericArguments()[0])
//                     && c.ServiceType.GetGenericArguments()[1].IsGenericType
//                     && c.ServiceType.GetGenericArguments()[1].GetGenericTypeDefinition() == typeof(PagedResponse<>));
//            
            container.RegisterSingleton<IHttpContextAccessor, HttpContextAccessor>();
               
//            container.Register<IQueryHandler<ProductFilterParam, PagedResponse<ProductDto>>, EfMapsterQueryHandler<ProductFilterParam, ProductDto>>(
//                Lifestyle.Scoped);
//
//            container.Register<IQueryHandler<int, ProductDto>, ProductDtoQueryHandler>(
//                Lifestyle.Scoped);
//
//            container.Register(typeof(ValidationResultDecorator<,>), 
//                typeof(ValidationResultDecorator<,>),
//                Lifestyle.Scoped);
//            
//            container.Register(typeof(ICommandHandler<,>), typeof(SomeCommandHandler<,>), Lifestyle.Scoped);
//            
//            container.RegisterConditional(
//                typeof(IQueryHandler<,>), typeof(ResultQueryHandler<,>),
//                Lifestyle.Scoped,
//                c => !c.Handled
//                     && typeof(Result).IsAssignableFrom(c.ServiceType.GetGenericArguments()[1]));      
//            
//            container.RegisterDecorator(
//                typeof(IQuery<,>),
//                typeof(ValidationCommandQueryDecorator<,>),
//                Lifestyle.Scoped,
//                c => typeof(Result).IsAssignableFrom(c.ServiceType.GetGenericArguments()[1]));
            
            container.RegisterDecorator(
                typeof(ICommandHandler<,>),
                typeof(ValidationHandlerDecorator<,>),
                Lifestyle.Scoped,
                c => typeof(Result).IsAssignableFrom(c.ServiceType.GetGenericArguments()[1]));
            // NOTE: Do prevent cross-wired instances as much as possible.
            // See: https://simpleinjector.org/blog/2016/07/
        }
   
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            IntegrateSimpleInjector(services);
            services
                .AddMiniProfiler();
                //.AddEntityFramework();
            
            services.AddMvc();
            services.AddDbContext<DemoContext>(options => options.UseSqlServer(DemoContext.ConnectionString));
            services.AddScoped<DbContext>(x => x.GetService<DemoContext>());
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
                    throw new NotImplementedException();
//                    var typeAccessor = TypeAccessor.Create(t);
//                    var dbContext = x.GetService<DemoContext>();
//                    return dbContext.Find(t, o);
                };
                
                return Func;
            });
            
      
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            
#if DEBUG
            app.UseDeveloperExceptionPage();
#endif
            InitializeContainer(app);
      
            app.UseMiniProfiler();
            app.UseMvc();

            
        }
    }
}