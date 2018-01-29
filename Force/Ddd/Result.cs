using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.Cqrs;

namespace Force.Ddd
{
    public class Result
    {
        public static implicit operator Result (Failure failure) => new Result(failure);
        
        public static bool operator false(Result result) => result.IsFaulted;
        
        public static bool operator true(Result result) => !result.IsFaulted;

        public static Result operator &(Result result1, Result result2)
            => Result.Combine(result1, result2);

        public static Result operator |(Result result1, Result result2)
            => result1.IsFaulted ? result2 : result1;
        
        public Failure Failure { get; internal set; }

        public bool IsFaulted => Failure != null;

        internal Result(IEnumerable<Result> results)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            if(!results.Any())
            {
                throw new ArgumentException(nameof(results));
            }

            // ReSharper disable once PossibleMultipleEnumeration
            var failures = results
                .Where(x => x.Failure != null)
                .Select(x => x.Failure)
                .ToArray();

            if (failures.Any())
            {
                Failure = new Failure(failures);
            }
        }
        
        internal Result()
        {           
        }

        internal Result(Failure failure)
        {
            Failure = failure;
        }
        
        public static Result Try(Action action)
        {
            try
            {
                action();
                return Result.Success;
            }
            catch (Exception e)
            {
                return new Result(e);
            }
        }
        
        public static Result<TDestination> Try<TSource, TDestination>(Func<TSource, TDestination> func, 
            TSource source)
        {
            try
            {
                return func(source);
            }
            catch (Exception e)
            {
                return new Result<TDestination>(e);
            }
        } 
        
        public static Result Combine(params Result[] results) => new Result(results);

        public static Result Combine(IEnumerable<Result> results) => new Result(results);

        public static Result Success = new Result(){};
        
        public static Result<T> Succeed<T> (T obj) => new Result<T>(obj);

        public static Result Fail(string failure) => Fail(new Failure(failure));
        
        public static Result Fail(Failure failure)
        {
            if (failure == null) throw new ArgumentNullException(nameof(failure));
            return new Result(failure);
        }

        public static Result<T> Fail<T>(string failure) => Fail<T>(new Failure(failure));
        
        public static Result<T> Fail<T>(Failure failure)
        {
            if (failure == null) throw new ArgumentNullException(nameof(failure));
            return new Result<T>(default(T))
            {
                Failure = failure
            };
        }
    }

    public class Result<T> : Result
    {
        public static implicit operator Result<T>(T value) => Succeed<T>(value);

        public static Result<T> operator &(Result<T> result1, Result<T> result2)
            => result1.IsFaulted
                ? result1
                : result2;
        
        internal T Value { get; }

        internal Result(IEnumerable<Result> results) : base(results)
        {
            
        }
        
        internal Result(Failure failure): base(failure)
        {
        }
        
        internal Result(T value)
        {
            Value = value;
            if (value == null)
            {
                Failure = new NullReferenceFailure(typeof(T));
            }
        }

        public Result<T> OnSuccess(Action<T> action)
        {
            if (!IsFaulted)
            {
                action(Value);
            }

            return this;
        }
        
        public Result<T> OnFailure(Action<Failure> action)
        {
            if (IsFaulted)
            {
                action(Failure);
            }

            return this;
        }
        
        public Result<T> Check(Func<T, bool> func, Func<T, Failure> onFailure)
        {
            if (!IsFaulted && !func(Value))
            {
                Failure = onFailure(Value);
            }
            
            return this;
        }
        
        public TDestination Return<TDestination>(Func<T, TDestination> success, Func<Failure, TDestination> failure)
            => IsFaulted
                ? failure(Failure)
                : success(Value);
    }
    
    public static class ResultExtensions
    {
        public static Result<TDestination> Select<TSource, TDestination>(this Result<TSource> source,
            Func<TSource, TDestination> selector)
            => source.IsFaulted
                ? new Result<TDestination>(source.Failure)
                : selector(source.Value);

        public static Result<TDestination> SelectMany<TSource, TDestination>(this Result<TSource> source,
            Func<TSource, Result<TDestination>> selector)
            => source.IsFaulted
                ? new Result<TDestination>(source.Failure)
                : selector(source.Value);
    
        public static Result<TDestination> SelectMany<TSource, TIntermediate, TDestination>(
            this Result<TSource> result,
            Func<TSource, Result<TIntermediate>> inermidiateSelector,
            Func<TSource, TIntermediate, TDestination> resultSelector)
            => result.SelectMany<TSource, TDestination>(s => inermidiateSelector(s)
                .SelectMany<TIntermediate, TDestination>(m => resultSelector(s, m)));      
    }

    public class Result<T, TFailure>: Result<T>
        where TFailure : Failure
    {
        internal Result(Failure failure) : base(failure)
        {
        }

        internal Result(T value) : base(value)
        {
        }

        public new TFailure Failure => (TFailure) base.Failure;
    }


}