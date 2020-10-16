using System;
using System.Collections.Generic;
using System.Linq;

namespace Force.Linq.Conventions
{
    public class FilterConventions
    {
        private FilterConventions(IEnumerable<IFilterConvention> filterConventions = null)
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
        
        public static FilterConventions Initialize(IEnumerable<IFilterConvention> conventions = null)
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("Filter conventions are already initialized");
            }

            _instance = new FilterConventions(conventions);
            return Instance;
        }

        private static FilterConventions _instance;
        
        public static FilterConventions Instance
        {
            get
            {
                if (_instance == null)
                {
                    InitializeWithDefaultConventions();
                }
                
                return _instance;
            }
        }
        
        private List<IFilterConvention> _filterConventions = new List<IFilterConvention>();

        internal IFilterConvention GetConvention(Type targetType, Type valueType) => 
            _filterConventions
                .FirstOrDefault(x =>
                   x.CanConvert(targetType, valueType));
    }
}