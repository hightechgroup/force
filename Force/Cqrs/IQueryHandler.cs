namespace Force.Cqrs
{
    public interface IQueryHandler<in TIn, out TOut>: IHandler<TIn, TOut>
        where TIn: IQuery<TOut>
    {        
    }
}