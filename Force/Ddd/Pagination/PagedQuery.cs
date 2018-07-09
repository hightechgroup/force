using System;

namespace Force.Ddd.Pagination
{
    public class PagedQuery<T> : Query<T>
    {
        public IPaging Paging { get; set; } = new Paging();
        
        public PagedQuery()
        {            
        }
        
        public PagedQuery(Spec<T> spec, Sorter<T> sorter, IPaging paging): base(spec, sorter)
        {
            Paging = paging ?? throw new ArgumentNullException(nameof(paging));
        }
    }
}