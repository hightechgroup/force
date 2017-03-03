using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Force.Cqrs;

namespace Force.AutoMapper
{
    public class ConventionalProfile : Profile
    {
        public static IDictionary<Type, Type[]> TypeMap;

        public static void Scan(params Assembly[] assemblies)
        {
            TypeMap = assemblies
                .SelectMany(x => x.GetTypes())
                .Where(x => 
                    x.GetTypeInfo().GetCustomAttribute<ProjectionAttribute>() != null
                    || x.GetTypeInfo().GetCustomAttribute<CommandAttribute>() != null
                    || x.GetTypeInfo().GetCustomAttribute<MessageAttribute>() != null)
                .GroupBy(x =>
                {
                    var attr = (ITypeAssociation)x.GetTypeInfo().GetCustomAttribute<ProjectionAttribute>()
                               ?? x.GetTypeInfo().GetCustomAttribute<CommandAttribute>()
                               ?? x.GetTypeInfo().GetCustomAttribute<MessageAttribute>();

                    return attr.EntityType;
                })
                .ToDictionary(k => k.Key, v => v.ToArray());
        }

        public ConventionalProfile()
        {
            if (TypeMap == null)
            {
                throw new InvalidOperationException("Use ConventionalProfile.Scan method first!");
            }

            foreach (var kv in TypeMap)
            {
                foreach (var v in kv.Value)
                {
                    ITypeAssociation attr = v.GetTypeInfo().GetCustomAttribute<ProjectionAttribute>();
                    if (attr != null)
                    {
                        CreateMap(kv.Key, v);
                    }

                    attr = (ITypeAssociation)v.GetTypeInfo().GetCustomAttribute<CommandAttribute>()
                           ?? v.GetTypeInfo().GetCustomAttribute<DomainEventAttribute>();

                    if (attr != null)
                    {
                        CreateMap(v, kv.Key).ConvertUsing(typeof(CreateOrUpdateCommandToEntityTypeConverter<,,>)
                            .MakeGenericType(kv.Key.GetTypeInfo().GetProperty("Id").PropertyType, v, kv.Key));
                    }
                }
            }
        }
    }
}
