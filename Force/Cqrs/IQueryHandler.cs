using System;
using System.Threading.Tasks;

namespace Force.Cqrs
{
    public interface IQueryHandler<in TCommand, out TResult> : IUseCaseHandler<TCommand, TResult>
        where TCommand: IUseCase<TResult>
    {       
    }

    public interface IAsyncQueryHandler<in TCommand, TResult> : IAsyncUseCaseHandler<TCommand, TResult>
        where TCommand: IUseCase<TResult>
    {        
    }
}
