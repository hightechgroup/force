using System.Threading;
using System.Threading.Tasks;

namespace Force
{
    public interface IUseHandler<in TIn>
    {
        void Handle(TIn input);
    }
    
    public interface IUseCaseHandler<in TIn, out TOut>
    {
        TOut Handle(TIn input);
    }

    public interface IAsyncUseCaseHandler<in TIn, TOut>
    {
        Task<TOut> Handle(TIn input, CancellationToken cancellationToken);
    }    
}