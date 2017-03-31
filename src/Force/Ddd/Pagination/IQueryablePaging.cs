namespace Force.Ddd.Pagination
{
    public interface IQueryablePaging<TEntity>: IPaging, IQueryableOrder<TEntity>
        where TEntity : class
    {
    }
}