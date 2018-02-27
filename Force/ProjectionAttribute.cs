using System;

namespace Force
{
    public class ProjectionAttribute: Attribute
    {
        public Type EntityType { get; }

        public ProjectionAttribute(Type entityType)
        {
            EntityType = entityType;
        }
    }
}