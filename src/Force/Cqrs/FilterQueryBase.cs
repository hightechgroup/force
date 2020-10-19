using System.Linq;
using Force.Ddd;
using Force.Linq;

namespace Force.Cqrs
{
    public class FilterQueryBase<T> 
        : IFilter<T>
        , ISorter<T>
    {
        public string Search { get; set; }

        public string Order { get; set; }

        public bool Asc { get; set; } = true;

        public virtual IOrderedQueryable<T> Sort(IQueryable<T> queryable) => Order == null
            ? queryable.OrderBy(_ => 0)
            : Asc 
                ? queryable.OrderBy(Order) 
                : queryable.OrderByDescending(Order);

        public virtual IQueryable<T> Filter(IQueryable<T> queryable)
        {
            // dynamic is for setting Build<T> the right type
            var spec = (Spec<T>)SpecBuilder<T>.Build((dynamic)this);
            return queryable.Where(spec);
        }
    }
}