using System;
using System.Collections.Generic;
using System.Linq;
using Force.Cqrs;
using Force.Ddd.Entities;
using Force.Ddd.Pagination;
using JetBrains.Annotations;

namespace Force.Components
{
    public class PaginatedAutoFilter<TKey, TDto>
        : IdPaging<TKey>
        , ILinqSpecification<TDto> 
        where TKey : class, IHasId<int>
        where TDto : class
    {
        public IDictionary<string, object> Filter { get; }

        public PaginatedAutoFilter()
        {
            Filter = new Dictionary<string, object>();
        }

        public PaginatedAutoFilter(int page, int take, [NotNull] IDictionary<string, object> filter)
            :base(page,take)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            Filter = filter;
        }
        public IQueryable<TDto> Apply(IQueryable<TDto> query)
            => query.ApplyDictionary(Filter);
    }
}
