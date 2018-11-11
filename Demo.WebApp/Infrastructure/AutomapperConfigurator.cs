using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.WebApp.Infrastructure
{
    public static class AutomapperConfigurator
    {
        private static Dictionary<(Type, Type), MethodInfo> _staticMappings 
            = new Dictionary<(Type, Type), MethodInfo>();

        public static void Configure(params Assembly[] assemblies)
        {
            ScanAssemblies(assemblies);
            Mapper.Initialize(Configure);
            Mapper.AssertConfigurationIsValid();
        }

        private static void ScanAssemblies(Assembly[] assemblies)
        {
            if (!assemblies.Any())
            {
                return;
            }
            
            foreach (var x in assemblies
                .SelectMany(x => x.GetTypes())
                .SelectMany(x => x.GetMethods())
                .Where(x => x.Name == "CreateMap" && x.IsStatic))
            {
                var paramters = x.GetParameters();
                if (paramters.Length != 1)
                {
                    continue;
                }

                var parameter = paramters.First();
                var parameterType = parameter.ParameterType;
                if (!parameterType.IsGenericType)
                {
                    continue;
                }

                var genericType = parameterType.GetGenericTypeDefinition();
                if (genericType != typeof(IMappingExpression<,>))
                {
                    continue;
                }

                var genericArgs = parameterType.GetGenericArguments();
                if (genericArgs[1] != x.DeclaringType)
                {
                    continue;
                }

                _staticMappings[(genericArgs[0], genericArgs[1])] = x;
            }
        }

        private static MethodInfo CreateMap = typeof(IProfileExpression)
            .GetMethods()
            .First(x => x.Name == "CreateMap" && x.IsGenericMethod);

        private static void Configure(IMapperConfigurationExpression mc)
        {
            foreach (var kv in _staticMappings)
            {
                var maping = CreateMap
                    .MakeGenericMethod(kv.Key.Item1, kv.Key.Item2)
                    .Invoke(mc, null);

                kv.Value.Invoke(null, new object[] {maping});
            }
        }        
    }
}