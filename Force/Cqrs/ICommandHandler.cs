using System.Threading.Tasks;

namespace Force.Cqrs
{
    public interface ICommandHandler<in TCommand, out TResult> : IUseCaseHandler<TCommand, TResult>
        where TCommand: IUseCase<TResult>
    {       
    }

    public interface IAsyncCommandHandler<in TCommand, TResult> : IAsyncUseCaseHandler<TCommand, TResult>
        where TCommand: IUseCase<TResult>
    {        
    }

    public interface ICommandHandler<in TCommand>
    {
        void Handle();
    }
}
