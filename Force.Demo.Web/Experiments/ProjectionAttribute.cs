using System;

namespace Force.Demo.Web
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