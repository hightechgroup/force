using System;

namespace Force.Ddd.Pagination
{
    public class PagedQuery<T> : Query<T>, IPaging
    {
        public PagedQuery()
        {            
        }
        
        public PagedQuery(Spec<T> spec, Sorter<T> sorter): base(spec, sorter)
        {
        }

        public int Page { get; set; }
        
        public int Take { get; set; }
    }
}