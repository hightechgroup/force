using System;
using System.ComponentModel.DataAnnotations;
using Force.Extensions;

namespace Demo.WebApp.Domain
{
    public abstract class NamedEntityBase: EntityBase<int>
    {
        private static DefaultStringLengthAttribute _attr = new DefaultStringLengthAttribute();
        
        protected NamedEntityBase()
        {}
        
        protected NamedEntityBase(string name)
        {
            Name = !string.IsNullOrEmpty(name) 
                   ? name
                   : throw new ArgumentNullException(nameof(name));
            if (!_attr.IsValid(name))
            {
                throw new ArgumentException(_attr.ErrorMessage, nameof(name));
            }
        }
        
        [DefaultStringLengthAttribute]
        [Required]
        public string Name { get; protected set; }
    }
}