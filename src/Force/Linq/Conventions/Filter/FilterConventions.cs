using System;
using System.Collections.Generic;
using System.Linq;

namespace Force.Linq.Conventions.Filter
{
    public class FilterConventions : ConventionsBase<IFilterConvention>
    {
        private FilterConventions(IEnumerable<IFilterConvention> filterConventions = null)
        {
            Conventions.AddRange(filterConventions ?? DefaultConventions);
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
    }
}