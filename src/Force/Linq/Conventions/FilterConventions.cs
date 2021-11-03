using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Force.Linq.Conventions
{
    public class FilterConventions
    {
        protected FilterConventions(IEnumerable<IFilterConvention> filterConventions = null)
        {
            _filterConventions.AddRange(filterConventions ?? DefaultConventions);
        }

        public static readonly IEnumerable<IFilterConvention> DefaultConventions =
            new[]
            {
                new StringConvention() as IFilterConvention, 
                new EnumerableConvention()
            };
        
        public static FilterConventions InitializeWithDefaultConventions()
            => Initialize(DefaultConventions);

        public static FilterConventions Initialize(IEnumerable<IFilterConvention> conventions = null,
            bool throwOnAlreadyInitialized = true)
        {
            if (_instance != null)
            {
                if (throwOnAlreadyInitialized)
                    throw new InvalidOperationException("Filter conventions are already initialized");
                return _instance;
            }

            var instance = new FilterConventions(conventions);
            var result = SetInstance(instance);
            
            if (instance != result)
            {
                if (throwOnAlreadyInitialized)
                    throw new InvalidOperationException("Filter conventions are already initialized");
            }

            return result;
        }

        private static FilterConventions _instance;

        protected static FilterConventions SetInstance(FilterConventions instance)
        {
            Interlocked.CompareExchange(ref _instance, instance, null);
            return _instance;
        }
        
        public static FilterConventions Instance
            => _instance ?? Initialize(throwOnAlreadyInitialized: false);

        private List<IFilterConvention> _filterConventions = new List<IFilterConvention>();

        internal IFilterConvention GetConvention(Type targetType, Type valueType) => 
            _filterConventions
                .FirstOrDefault(x =>
                   x.CanConvert(targetType, valueType));
    }
}