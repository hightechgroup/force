using System;

namespace Force.AutoMapper
{
    public enum MapOptions
    {
        EntityToDto= 0x0,
        DtoToEntity = 0x1

    }

    public class AutoMapAttribute : Attribute
    {
        public Type EntityType { get; }

        public MapOptions MapOptions { get; }

        public AutoMapAttribute(Type entityType, MapOptions mapOptions = MapOptions.EntityToDto)
        {
            EntityType = entityType;
            MapOptions = mapOptions;
        }
    }
}