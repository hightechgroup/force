using System.Threading.Tasks;

namespace Force.Cqrs
{
    public interface ICommandHandler<in TCommand, out TResult> : IRequestHandler<TCommand, TResult>
    {       
    }

    public interface IAsyncCommandHandler<in TCommand, TResult> : IAsyncRequestHandler<TCommand, TResult>
    {        
    }

    public interface ICommandHandler<in TCommand>
    {
        void Handle();
    }
}
