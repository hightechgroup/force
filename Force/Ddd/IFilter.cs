namespace Force.Ddd
{
    public interface IFilter<T>    
    {
        Spec<T> Spec { get; }
    }
}