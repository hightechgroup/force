using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Force.Infrastructure
{
    public class SynchronizedConcurrentDictionary<TKey, TValue> : ConcurrentDictionary<TKey, TValue>
    {
        private readonly ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();

        public new TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            TValue result;

            _cacheLock.EnterWriteLock();
            try
            {
                result = base.GetOrAdd(key, valueFactory);
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }

            return result;
        }
    }
}