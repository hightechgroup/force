namespace Force.Ddd.Specifications
{
    public interface ISpecification<in T>
    {
        bool IsSatisfiedBy(T o);
    }
}