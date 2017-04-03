using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace Force.AutoMapper
{
    public class AttributeProfile : Profile
    {
        public IDictionary<Type, Type[]> TypeMap;

        public AttributeProfile(params Assembly[] assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            TypeMap = assemblies
                .SelectMany(x => x.GetTypes())
                .Where(x => x.GetTypeInfo().GetCustomAttribute<AutoMapAttribute>() != null)
                .GroupBy(x => x.GetTypeInfo().GetCustomAttribute<AutoMapAttribute>().EntityType)
                .ToDictionary(k => k.Key, v => v.ToArray());

            foreach (var kv in TypeMap)
            {
                foreach (var v in kv.Value)
                {
                    var attr = v.GetTypeInfo().GetCustomAttribute<AutoMapAttribute>();

                    if (attr.MapOptions != MapOptions.DtoToEntity)
                    {
                        CreateMap(kv.Key, v);
                    }

                    if (attr.MapOptions != MapOptions.EntityToDto)
                    {
                        var hasParameterlessCtor = kv.Key
                            .GetTypeInfo()
                            .DeclaredConstructors.Any(x => !x.GetParameters().Any());

                        if (!hasParameterlessCtor)
                            throw new InvalidOperationException(
                                $"Type {kv.Key} should provide parameterless constructor (public, protected or private) in order to be mapped");

                        CreateMap(v, kv.Key)
                            .ConvertUsing(typeof(DtoToEntityTypeConverter<,,>)
                                .MakeGenericType(kv.Key.GetTypeInfo().GetProperty("Id").PropertyType, v, kv.Key));
                    }
                }
            }
        }
    }
}