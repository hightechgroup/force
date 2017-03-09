using System.Threading.Tasks;

namespace Force.Cqrs
{
    public interface IQuery<out TOutput>
    {
        TOutput Ask();
    }

    public interface IQuery<in TSpecification, out TOutput>
    {
        TOutput Ask(TSpecification spec);
    }

    public interface IAsyncQuery<TOutput>
        : IQuery<Task<TOutput>>
    {
    }

    public interface IAsyncQuery<in TSpecification, TOutput>
        : IQuery<TSpecification, Task<TOutput>>
    {
    }
}
