using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WebApplication.Services
{
    public abstract class TwoPhaseCommitServiceBase<T, TException>
        where TException: Exception
    {
        private readonly ILogger _logger;

        public TwoPhaseCommitServiceBase(ILogger logger)
        {
            _logger = logger;
        }

        private async Task DoInTransaction(T obj, Func<T, Task> tryMethod)
        {
            try
            {
                await tryMethod(obj);
                await Commit();
            }
            catch (TException e1)
            {
                try
                {
                    await CatchAsync(obj, e1);
                }
                catch (Exception e2)
                {
                    _logger.LogError(e2.Message, e2);
                }

                throw;
            }
        }

        public async Task InTransactionAsync(T obj)
        {
            await DoInTransaction(obj, TryAsync);
        }
        
        public async Task InTransactionAsync(T obj, Func<T, Func<T, Task>, Task> action)
        {
            await DoInTransaction(obj, x => action(x, TryAsync));
        }

        protected abstract Task Commit();
        
        protected abstract Task TryAsync(T obj);
        
        protected abstract Task CatchAsync(T obj, TException e);
    }
}