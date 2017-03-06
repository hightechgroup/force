using System;

namespace Force.Meta
{
    public class SpecificationAttribute : Attribute, ITypeAssociation
    {
        public Type EntityType { get; }

        public SpecificationAttribute(Type entityType)
        {
            EntityType = entityType;
        }
    }
}