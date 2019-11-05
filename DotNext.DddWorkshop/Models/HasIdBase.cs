using System;
using System.ComponentModel.DataAnnotations;

namespace DotNext.DddWorkshop.Models
{
    public abstract class HasIdBase: HasIdBase<int>
    {}
    
    public abstract class HasIdBase<TKey>
        where TKey: IEquatable<TKey>
    {
        [Key, Required]
        public virtual TKey Id { get; set; }
    }
}