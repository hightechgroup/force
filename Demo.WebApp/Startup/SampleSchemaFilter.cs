using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Demo.WebApp.Startup
{
    public class SampleSchemaFilter : ISchemaFilter
    {
        public void Apply(Schema schema, SchemaFilterContext context)
        {
            var typeInfo = context.SystemType.GetTypeInfo();
            var dn = typeInfo.GetCustomAttribute<DisplayAttribute>();
            if (dn != null)
            {
                schema.Properties["id"].Extensions["DisplayDada"] = dn.Name;
                schema.Extensions["DisplayDada"] = dn.Name;
            }
        }
    }
}