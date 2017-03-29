using System;

namespace Force.AutoMapper
{
    public enum MapOptions: byte
    {
        Both, EntityToDto, DtoToEntity
    }

    public class AutomapAttribute : Attribute
    {
        public Type EntityType { get; }

        public MapOptions MapOptions { get; }

        public AutomapAttribute(Type entityType, MapOptions mapOptions)
        {
            EntityType = entityType;
            MapOptions = mapOptions;
        }
    }
}