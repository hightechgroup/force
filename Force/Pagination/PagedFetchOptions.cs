using System;

namespace Force.Ddd.Pagination
{
    public class PagedFetchOptions<T> : FetchOptions<T>, IPaging
    {
        public PagedFetchOptions()
        {            
        }
        
        public PagedFetchOptions(Spec<T> spec, Sorter<T> sorter): base(spec, sorter)
        {
        }

        public int Page { get; set; }
        
        public int Take { get; set; }
    }
}