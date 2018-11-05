namespace Force.Ddd
{
    public interface IOrder<T>
    {
        Sorter<T> Sorter { get; }
    }
}