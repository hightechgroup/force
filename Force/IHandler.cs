using System.Threading;
using System.Threading.Tasks;

namespace Force
{
    public interface IHandler<in TIn>
    {
        void Handle(TIn input);
    }
    
    public interface IRequestHandler<in TIn, out TOut>
    {
        TOut Handle(TIn input);
    }

    public interface IAsyncRequestHandler<in TIn, TOut>
    {
        Task<TOut> Handle(TIn input, CancellationToken cancellationToken);
    }    
}