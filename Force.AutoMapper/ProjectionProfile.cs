using System;
using System.Linq;
using System.Reflection;

namespace AutoMapper.Extensions
{
    public class ProjectionProfile: Profile
    {
        private static MethodInfo CreateMapMethod = typeof(Profile)
            .GetMethods()
            .First(x => x.IsGenericMethod && x.Name == "CreateMap");
            
        private static bool IsMapMethod(MethodInfo method, Type sourceType, Type destType)
        {
            if (!method.IsStatic || !method.IsPublic)
            {
                return false;
            }

            var parameters = method.GetParameters();
            if (parameters.Length > 1)
            {
                return false;
            }

            var parameter = parameters.First();
            if (!parameter.ParameterType.IsGenericType
                || parameter.ParameterType.GetGenericTypeDefinition() != typeof(IMappingExpression<,>))
            {
                return false;
            }

            var arguments = parameter.ParameterType.GetGenericArguments();
            if (arguments[0] != sourceType || arguments[1] != destType)
            {
                throw new InvalidOperationException(
                    $"Method {method.DeclaringType.Name}.{method.Name} has IMappingExpression<{arguments[0]},{arguments[1]}> " +
                    $"argument type. The argument must be of IMappingExpression<{sourceType},{destType}> type instead.");
            }

            return true;
        }
        
        public ProjectionProfile(params Assembly[] assemblies)
        {
            var types = assemblies
                .SelectMany(x => x.GetExportedTypes())
                .Where(x => x.GetCustomAttribute<ProjectionAttribute>() != null)
                .ToList();

            foreach (var destType in types)
            {
                var sourceType = destType.GetCustomAttribute<ProjectionAttribute>().Type;
               
                
                var mapMethods = destType
                    .GetMethods()
                    .Where(x => IsMapMethod(x, sourceType, destType))
                    .ToList();

                var mapping = CreateMapMethod
                    .MakeGenericMethod(sourceType, destType)
                    .Invoke(this, null);
                
                foreach (var method in mapMethods)
                {
                    method.Invoke(null, new[] {mapping});
                }
            }
        }
    }
}