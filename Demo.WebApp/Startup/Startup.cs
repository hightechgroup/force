using Demo.WebApp.Data;
using Demo.WebApp.Infrastructure;
using Force.AspNetCore.Mvc;
using Force.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Demo.WebApp.Startup
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMiniProfiler(options => options.RouteBasePath = "/profiler") // http://localhost:5000/profiler/results-index
                .AddEntityFramework();
            
            AutomapperConfigurator.Configure(GetType().Assembly);
            services.AddDbContext<DemoAppDbContext>(o => o.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddScoped<DbContext, DemoAppDbContext>();

            services.AddMvc(options =>
                {
                    // add custom binder to beginning of collection
                    options.ModelBinderProviders.Insert(0, new IdModelBinderProvider());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    //options.SerializerSettings.Converters.Add(new PagedEnumerableJsonConverter());
                });
            
            services.AddSwaggerGen(c =>
            {
                var f = c.DocumentFilterDescriptors;
                var p = c.ParameterFilterDescriptors;
                
                c.SwaggerDoc("v1", new Info { Title = "API", Version = "v1" });
                c.SchemaFilter<SampleSchemaFilter>();
//                c.MapType<string>(() => new Schema
//                {
//                    Type = "string!!",
//                    Format = "FFF",
//                    Default = "Default",
//                    Description = "Desc",
//                    Extensions = { {"key", "value"}}
//                });
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var ob = new DbContextOptionsBuilder<DemoAppDbContext>();
            ob.UseSqlServer(Configuration.GetConnectionString("Default"));
            
            using (var dbContext = new DemoAppDbContext(ob.Options))
            {
                dbContext.Database.Migrate();
                Seed.Run(dbContext);
            }            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            
            app.UseSwagger();
            app.UseMiniProfiler();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });
            
            //app.UseHttpsRedirection();
            app.UseMvc();
            
//            app.UseSpa(spa =>
//            {
//                // To learn more about options for serving an Angular SPA from ASP.NET Core,
//                // see https://go.microsoft.com/fwlink/?linkid=864501
//
//                spa.Options.SourcePath = "ClientApp";
//
//                if (env.IsDevelopment())
//                {
//                    spa.UseAngularCliServer(npmScript: "start");
//                }
//            });
        }
    }
}