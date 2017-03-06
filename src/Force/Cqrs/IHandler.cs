using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Force.Cqrs
{
    /// <summary>
    /// Command or Domain Event handler
    /// </summary>
    /// <typeparam name="TInput">Command or Domain Event type</typeparam>
    [PublicAPI]
    public interface IHandler<in TInput>
    {
         void Handle(TInput input);
    }

    /// <summary>
    /// Command or Domain Event handler
    /// </summary>
    /// <typeparam name="TInput">Command or Domain Event type</typeparam>
    /// <typeparam name="TOutput">Operation result type</typeparam>
    [PublicAPI]
    public interface IHandler<in TInput, out TOutput>
    {
        TOutput Handle(TInput command);
    }

    /// <summary>
    /// Async Command or Domain Event handler
    /// </summary>
    /// <typeparam name="TInput">Command or Domain Event type</typeparam>
    [PublicAPI]
    public interface IAsyncHandler<in TInput> : IHandler<TInput, Task>
    {
    }

    /// <summary>
    /// Async Command or Domain Event handler
    /// </summary>
    /// <typeparam name="TInput">Command or Domain Event type</typeparam>
    /// <typeparam name="TOutput"></typeparam>
    [PublicAPI]
    public interface IAsyncHandler<in TInput, TOutput> : IHandler<TInput, Task<TOutput>>
    {
    }
}
