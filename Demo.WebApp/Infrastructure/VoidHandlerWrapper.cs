using Force;

namespace Demo.WebApp.Infrastructure
{
    public sealed class Void
    {      
        private Void(){}
    }
    
    public class VoidHandlerWrapper<T>: IHandler<T, Void>
    {
        public VoidHandlerWrapper(IHandler<T> handler)
        {
        }

        public Void Handle(T input)
        {
            throw new System.NotImplementedException();
        }
    }
}