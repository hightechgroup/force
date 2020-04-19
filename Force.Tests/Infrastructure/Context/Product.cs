using System.ComponentModel.DataAnnotations;
using Force.Ddd;
using Force.Extensions;

namespace Force.Tests.Infrastructure.Context
{
    [Display(Name = "Product")]
    public class Product : HasNameBase
    {
        [Required]
        public Category Category { get; set; }

        protected Product() : base()
        {
        }

        public Product(Category category, string name) : base(name)
        {
            Category = category;
            this.EnsureInvariant();
        }
    }
}