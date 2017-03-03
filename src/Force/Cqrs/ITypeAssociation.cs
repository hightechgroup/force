using System;

namespace Force.Cqrs
{
    public interface ITypeAssociation
    {
        Type EntityType { get; }
    }
}