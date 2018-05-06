using System.Linq;

namespace Force.Ddd.Pagination
{
    public class SmartPaging<T> : Paging, ISmartPaging<T>
        where T : class, IHasId
    {
        public string OrderBy { get; set; }

        private AutoFilter<T> _auto;

        protected AutoFilter<T> AutoFilter => _auto ?? (_auto = new AutoFilter<T>(this, OrderBy));
        
        public virtual IQueryableOrder<T> Order => AutoFilter;

        public virtual IQueryableFilter<T> Filter => AutoFilter;
    }

    public class SmartPaging<TEntity, TProjection>
        : SmartPaging<TProjection>
        , ISmartPaging<TEntity, TProjection>
        where TProjection : class, IHasId
        where TEntity : class, IHasId
    {
        public virtual Spec<TEntity> Spec { get; }
    }
}