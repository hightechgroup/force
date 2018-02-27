using Force.Cqrs;
using Force.Ddd;

namespace Force.Demo.Web
{
    public class ResultQueryHandler<TSource, TDestination>
        : IQueryHandler<TSource, Result<TDestination>>
    {
        private readonly IQueryHandler<TSource, TDestination> _queryHandler;

        public ResultQueryHandler(IQueryHandler<TSource, TDestination> queryHandler)
        {
            _queryHandler = queryHandler;
        }

        public Result<TDestination> Handle(TSource param)
            => Result.Succeed(_queryHandler.Handle(param));
    }
}