using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Force.Ddd;
using Force.Linq;

namespace Force.Cqrs
{
    public class FilterQuery<T> 
        : IQuery<IEnumerable<T>>
        , IFilter<T>
        , ISorter<T>
    {
        public string Search { get; set; }

        public string Order { get; set; }

        public bool Asc { get; set; } = true;

        public IOrderedQueryable<T> Sort(IQueryable<T> queryable) => Asc
            ? queryable.OrderBy(Order)
            : queryable.OrderByDescending(Order);

        public IQueryable<T> Filter(IQueryable<T> queryable)
        {
            var spec = SpecBuilder<T>.Build(this);
            return queryable.Where(spec);
        }
    }
}