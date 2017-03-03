using System;

namespace Force.Cqrs
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