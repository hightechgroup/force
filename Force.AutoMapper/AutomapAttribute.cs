using System;

namespace Force.AutoMapper
{
    public enum MapOptions: byte
    {
        EntityToDto, DtoToEntity, Both
    }

    public class AutoMapAttribute : Attribute
    {
        public Type EntityType { get; }

        public MapOptions MapOptions { get; }

        public AutoMapAttribute(Type entityType, MapOptions mapOptions)
        {
            EntityType = entityType;
            MapOptions = mapOptions;
        }
    }
}