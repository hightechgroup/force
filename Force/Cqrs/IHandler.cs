namespace Force.Cqrs
{
    public interface IHandler<in TIn>
    {
        void Handle(TIn input);
    }
    
    public interface IHandler<in TIn, out TOut>
    {
        TOut Handle(TIn input);
    } 
}