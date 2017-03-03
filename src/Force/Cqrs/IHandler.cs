using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Force.Cqrs
{
    [PublicAPI]
    public interface IHandler<in TInput>
    {
         void Handle(TInput input);
    }

    [PublicAPI]
    public interface IHandler<in TInput, out TOutput>
    {
        TOutput Handle(TInput command);
    }

    [PublicAPI]
    public interface IAsyncHandler<in TInput>
        : IHandler<TInput, Task>
    {
    }

    [PublicAPI]
    public interface IAsyncHandler<in TInput, TOutput>
        : IHandler<TInput, Task<TOutput>>
    {
    }
}
