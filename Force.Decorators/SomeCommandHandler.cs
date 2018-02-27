using Force.Cqrs;
using Force.Ddd;

namespace Force.Demo.Web
{
    public class SomeCommandHandler<TSource, TDestination> : ICommandHandler<TSource, Result<TDestination>>
    {
        public Result<TDestination> Handle(TSource command)
        {
            return Result.Succeed(default(TDestination));
        }
    }
}