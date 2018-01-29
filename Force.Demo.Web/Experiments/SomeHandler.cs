using Force.Cqrs;
using Force.Ddd;

namespace Force.Demo.Web
{
    public class SomeHandler<TSource, TDestination> : IHandler<TSource, Result<TDestination>>
    {
        public Result<TDestination> Handle(TSource command)
        {
            return Result.Succeed(default(TDestination));
        }
    }
}