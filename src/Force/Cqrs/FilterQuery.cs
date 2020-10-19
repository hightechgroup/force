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
            , ISearch<T>
    {
        public string Search { get; set; }

        public string SearchBy { get; set; }

        public string Order { get; set; }

        public bool Asc { get; set; } = true;

        public virtual IOrderedQueryable<T> Sort(IQueryable<T> queryable) => Asc
            ? queryable.OrderBy(Order)
            : queryable.OrderByDescending(Order);

        public virtual IQueryable<T> Filter(IQueryable<T> queryable)
        {
            // dynamic is for setting Build<T> the right type
            var spec = (Spec<T>) SpecBuilder<T>.Build((dynamic) this);
            return queryable.Where(spec);
        }

        public virtual IQueryable<T> SearchItem(IQueryable<T> queryable)
        {
            var spec = (Spec<T>) (SearchBy.ToLower() == "all"
                ? SpecBuilder<T>.BuildSearch((dynamic) this)
                : SpecBuilder<T>.BuildSearchBy((dynamic) this));

            return queryable.Where(spec);
        }
    }
}