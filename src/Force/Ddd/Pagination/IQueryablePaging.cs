namespace Force.Ddd.Pagination
{
    public interface IQueryablePaging<TEntity>: IPaging, IQueryableOrderBy<TEntity>
        where TEntity : class
    {
    }
}