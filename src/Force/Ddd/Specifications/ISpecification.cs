using JetBrains.Annotations;

namespace Force.Ddd.Specifications
{
    public interface ISpecification<in T>
    {
        bool IsSatisfiedBy([NotNull]T o);
    }
}