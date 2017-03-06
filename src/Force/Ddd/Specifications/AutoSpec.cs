using System.Linq;
using System.Linq.Dynamic.Core;
using Force.Cqrs;
using Force.Ddd.Entities;
using Force.Ddd.Pagination;
using Force.Extensions;

namespace Force.Ddd.Specifications
{
    public class AutoSpec<TProjection>
        : IPaging
        , ILinqSpecification<TProjection>
        , ILinqOrderBy<TProjection>
        where TProjection : class, IHasId
    {
        public virtual IQueryable<TProjection> Apply(IQueryable<TProjection> query) => GetType()
            .GetPublicProperties()
            .Where(x => typeof(TProjection).GetPublicProperties().Any(y => x.Name == y.Name))
            .Aggregate(query, (current, next) =>
            {
                var val = next.GetValue(this);
                if (val == null) return current;
                return current.Where(next.PropertyType == typeof(string)
                       ? $"{next.Name}.StartsWith(@0)"
                       : $"{next.Name}=@0", val);
            });

        IOrderedQueryable<TProjection> ILinqOrderBy<TProjection>.Apply(IQueryable<TProjection> queryable)
            => !string.IsNullOrEmpty(OrderBy) ? queryable.OrderBy(OrderBy) : queryable.OrderBy(x => x.Id);

        public int Page { get; set; } = 1;

        public int Take { get; set; } = 30;

        public string OrderBy { get; set; }
    }

}