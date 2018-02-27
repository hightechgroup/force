using System;
using System.Threading.Tasks;

namespace Force.Cqrs
{
    public interface IQueryHandler<in TCommand, out TResult> : IRequestHandler<TCommand, TResult>
    {       
    }

    public interface IAsyncQueryHandler<in TCommand, TResult> : IAsyncRequestHandler<TCommand, TResult>
    {        
    }
}
