using System;

namespace Force.Ddd
{
    public class Result<TSuccess, TFailure>
    {
        private readonly TFailure _failure;
        private readonly TSuccess _success;
        private readonly bool _isSuccess;
        
        public static implicit operator Result<TSuccess, TFailure> (TFailure failure)
            => new Result<TSuccess, TFailure>(failure);

        public static implicit operator Result<TSuccess, TFailure> (TSuccess success)
            => new Result<TSuccess, TFailure>(success);
        
        public Result(TSuccess success)
        {
            _success = success;
            _isSuccess = true;
        }

        public Result(TFailure failure)
        {
            _failure = failure;
        }

        public bool IsFaulted => !_isSuccess;

        public TResult Match<TResult>(Func<TSuccess, TResult> success, Func<TFailure, TResult> failure)
            => _isSuccess ? success(_success) : failure(_failure);
    }
    
    public class Result<T>: Result<T, string>
    {
        public Result(T success) : base(success)
        {
        }

        public Result(string failure) : base(failure)
        {
        }
        
        public Result(Exception e) : base(e.Message)
        {
        }
    }

    public static class ResultExtensions
    {
        public static Result<TDestination, TFailure> Select<TFailure, TSource, TDestination>(
            this Result<TSource, TFailure> source,
            Func<TSource, TDestination> selector)
            => source.Match<Result<TDestination, TFailure>>(x => selector(x), x => x);

        public static Result<TDestination, TFailure> SelectMany<TFailure, TSource, TDestination>(
            this Result<TSource, TFailure> source,
            Func<TSource, Result<TDestination, TFailure>> selector)
            => source.Match(selector, x => x);
    
        public static Result<TDestination, TFailure>
            SelectMany<TFailure, TSource, TIntermediate, TDestination>(
                this Result<TSource, TFailure> result,
                Func<TSource, Result<TIntermediate, TFailure>> inermidiateSelector,
                Func<TSource, TIntermediate, TDestination> resultSelector)
            => result
                .SelectMany(s => inermidiateSelector(s)
                .SelectMany<TFailure, TIntermediate, TDestination>(m => resultSelector(s, m)));
    }
}