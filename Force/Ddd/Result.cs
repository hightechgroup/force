using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Force.Ddd
{
    public class Result
    {
        public Failure Failure { get; private set; }

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
        
        public static Result Combine(params Result[] results) => new Result(results);

        public static Result Combine(IEnumerable<Result> results) => new Result(results);

        public static Result Success = new Result(){};
        
        public static Result Succeed<T> (T obj) => new Result<T>(obj);
        
        public static Result Fail(Failure failure)
        {
            if (failure == null) throw new ArgumentNullException(nameof(failure));
            return new Result()
            {
                Failure = failure
            };
        }

        public static Result<T> Fail<T>(Failure failure)
        {
            if (failure == null) throw new ArgumentNullException(nameof(failure));
            return new Result<T>(default(T))
            {
                Failure = failure
            };
        }
    }

    public sealed class Result<T> : Result
    {
        public T Value { get; }

        internal Result(T value)
        {
            Value = value;
        }

        public TOut Return<TOut>(Func<T, TOut> success, Func<Failure, TOut> failure)
            => IsFaulted ? failure(Failure) : success(Value);
    }
}