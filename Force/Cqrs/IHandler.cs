using System.Threading.Tasks;

namespace Force.Cqrs
{
    /// <summary>
    /// Command or Domain Event handler
    /// </summary>
    /// <typeparam name="TInput">Command or Domain Event type</typeparam>
    public interface IHandler<in TInput>
    {
         void Handle(TInput input);
    }

    /// <summary>
    /// Command or Domain Event handler
    /// </summary>
    /// <typeparam name="TInput">Command or Domain Event type</typeparam>
    /// <typeparam name="TOutput">Operation result type</typeparam>
    public interface IHandler<in TInput, out TOutput>
    {
        TOutput Handle(TInput command);
    }

    /// <summary>
    /// Async Command or Domain Event handler
    /// </summary>
    /// <typeparam name="TInput">Command or Domain Event type</typeparam>
    public interface IAsyncHandler<in TInput> : IHandler<TInput, Task>
    {
    }

    /// <summary>
    /// Async Command or Domain Event handler
    /// </summary>
    /// <typeparam name="TInput">Command or Domain Event type</typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public interface IAsyncHandler<in TInput, TOutput> : IHandler<TInput, Task<TOutput>>
    {
    }
}
