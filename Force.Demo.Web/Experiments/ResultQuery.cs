using Force.Cqrs;
using Force.Ddd;

namespace Force.Demo.Web
{
    public class ResultQuery<TSource, TDestination>: IQuery<TSource, Result<TDestination>>
    {
        private readonly IQuery<TSource, TDestination> _query;

        public ResultQuery(IQuery<TSource, TDestination> query)
        {
            _query = query;
        }

        public Result<TDestination> Ask(TSource spec)
            => Result.Try(_query.Ask, spec);
    }
}