using System;
using System.Collections.Generic;
using System.Linq;

namespace Force.Linq.Conventions.Search
{
    public class SearchConvensions : ConventionsBase<ISearchConvention>
    {
        private SearchConvensions(IEnumerable<ISearchConvention> searchConventions = null)
        {
            Conventions.AddRange(searchConventions ?? DefaultConventions);
        }

        public static readonly IEnumerable<ISearchConvention> DefaultConventions = new[]
        {
            new StringConvension()
        };

        public static SearchConvensions InitializeWithDefaultConventions()
            => Initialize(DefaultConventions);

        public static SearchConvensions Initialize(IEnumerable<ISearchConvention> conventions = null)
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("Search conventions are already initialized");
            }

            _instance = new SearchConvensions(conventions);
            return Instance;
        }

        private static SearchConvensions _instance;

        public static SearchConvensions Instance
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