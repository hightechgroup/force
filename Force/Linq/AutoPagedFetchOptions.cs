using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Force.Ddd.Pagination;

namespace Force.Ddd
{
    public class AutoPagedFetchOptions<T>: PagedFetchOptions<T>
    {
        private static ValidationResult[] Empty = { };
        
        private Spec<T> _spec;

        private Sorter<T> _sorter;
        
        public string OrderBy { get; set; }

        public Spec<T> Spec => _spec ?? (_spec = this.ToSpec<T>());

        public Sorter<T> Sorter => !string.IsNullOrEmpty(OrderBy) 
            ? _sorter ?? (_sorter = new Sorter<T>(OrderBy))
            : null;

        public AutoPagedFetchOptions()
        {            
        }
    }
}