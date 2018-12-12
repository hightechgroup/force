using System.Threading;
using System.Threading.Tasks;

namespace Force
{
    public interface IHandler<in TIn>
    {
        void Handle(TIn input);
    }
    
    public interface IHandler<in TIn, out TOut>
    {
        TOut Handle(TIn input);
    } 
}