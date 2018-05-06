namespace Force.Ddd.Pagination
{
    public interface ISmartPaging<T>: IPaging 
        where T : class
    {
        IQueryableOrder<T> Order { get; }
        
        IQueryableFilter<T> Filter { get; }
    }

    public interface ISmartPaging<TEntity, TProjection> : ISmartPaging<TProjection>
        where TProjection : class
        where TEntity : class, IHasId
    {
        Spec<TEntity> Spec { get; }
    }
}