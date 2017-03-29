namespace Force.Ddd.Pagination
{
    public interface IPaging
    {
        int Page { get; }

        int Take { get; }
    }
}